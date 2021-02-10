using Application.Exceptions;
using Application.Interfaces;
using Application.Messages;
using Kubernox.Service.Hubs;
using Domain.Entities;
using Infrastructure.Contracts.Request;
using Infrastructure.Contracts.Response;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using ProxmoxVEAPI.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubernox.Service.Business
{
    public class ClusterBusiness : IClusterBusiness
    {
        private readonly IClusterRepository clusterRepository;
        private readonly IClusterNodeRepository clusterNodeRepository;
        private readonly IDatacenterRepository datacenterRepository;
        private readonly ISshKeyRepository sshKeyRepository;
        private readonly IQueueService queueService;
        private readonly ITemplateRepository templateRepository;
        private readonly IConfiguration configuration;
        private readonly ITraefikRedisStore traefikCache;
        private readonly IHubContext<AppHub, IAppHub> hubContext;

        public ClusterBusiness(IClusterRepository clusterRepository, IDatacenterRepository datacenterRepository, 
            ISshKeyRepository sshKeyRepository, IClusterNodeRepository clusterNodeRepository, 
            IQueueService queueService, ITemplateRepository templateRepository, 
            IConfiguration configuration, ITraefikRedisStore traefikCache, IHubContext<AppHub, IAppHub> hubContext)
        {
            this.clusterRepository = clusterRepository;
            this.datacenterRepository = datacenterRepository;
            this.sshKeyRepository = sshKeyRepository;
            this.clusterNodeRepository = clusterNodeRepository;
            this.queueService = queueService;
            this.templateRepository = templateRepository;
            this.configuration = configuration;
            this.traefikCache = traefikCache;
            this.hubContext = hubContext;
            ConfigureClient.Initialise(configuration["Proxmox:Uri"], configuration["Proxmox:Token"]);
        }

        public async Task<bool> CreateClusterAsync(ClusterCreateRequest request)
        {
            if ((await clusterRepository.ReadAsync(c => c.Name == request.Name)) != null)
                throw new DuplicateException();

            var selectedSshKey = await sshKeyRepository.ReadAsync(request.SshKeyId);
            var selectedNode = await datacenterRepository.ReadAsync(f => f.Id == request.DeployNodeId);
            var template = await templateRepository.ReadAsync(t => t.Id == request.SelectedTemplate);
            var existingCluster = await clusterRepository.ReadsAsync(c => c.ProxmoxNodeId == request.DeployNodeId);

            var baseIp = existingCluster.Any() ? GetBasedRangeIp(existingCluster) : 10;
            var baseId = existingCluster.Any() ? ExtractBaseId(await clusterRepository.GetMaxOrder()) : 3000;

            if (baseIp > 0 && baseId > 0)
            {
                var newCluster = new Cluster()
                {
                    Cpu = request.Cpu,
                    Name = $"k3s-master-{request.Name}",
                    Node = request.Node,
                    Storage = request.Storage,
                    User = "root",
                    ProxmoxNodeId = selectedNode.Id,
                    OrderId = baseId,
                    Memory = request.Memory,
                    Ip = $"10.0.{selectedNode.Id}.{baseIp}",
                    SshKey = selectedSshKey.Public
                };

                if ((await clusterRepository.InsertClusterAsync(newCluster)) > 0)
                {
                    var nodeList = new List<ClusterNode>();

                    for (int i = 0; i < request.Node; i++)
                    {
                        nodeList.Add(new ClusterNode()
                        {
                            ClusterId = newCluster.Id,
                            OrderId = baseId + i + 1,
                            Name = $"k3s-node{i + 1}-{request.Name}",
                            Ip = $"10.0.{selectedNode.Id}.{baseIp + i + 1}",
                        });
                    }

                    if ((await clusterNodeRepository.InsertClusterNodesAsync(nodeList.ToArray()) == request.Node))
                    {
                        ClusterCreateMessage message = GenerateQueueMessage(request, newCluster, selectedSshKey, selectedNode, template, baseIp, baseId);
                        queueService.QueueClusterCreation(message);

                        await InjectClusterCacheConfigurationAsync(newCluster);
                    }

                    return true;
                }
            }

            return false;
        }

        private async Task InjectClusterCacheConfigurationAsync(Cluster newCluster)
        {
            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>($"traefik/http/routers/{newCluster.Name}/rule", $"Host(`{newCluster.Name}.ice-artefact.com`)"));
            values.Add(new KeyValuePair<string, string>($"traefik.http.routers.{newCluster.Name}.entrypoints/0", $"web"));
            values.Add(new KeyValuePair<string, string>($"traefik.http.routers.{newCluster.Name}.entrypoints/1", $"webSecure"));
            values.Add(new KeyValuePair<string, string>($"traefik/http/routers/{newCluster.Name}/service", $"{newCluster.Name}-http-lb"));
            values.Add(new KeyValuePair<string, string>($"traefik/http/services/{newCluster.Name}-http-lb/loadbalancer/servers/0/url", $"http://{newCluster.Ip}:80"));
            values.Add(new KeyValuePair<string, string>($"traefik/http/services/{newCluster.Name}-http-lb/loadbalancer/servers/1/url", $"https://{newCluster.Ip}:443"));
            values.Add(new KeyValuePair<string, string>($"traefik/http/services/{newCluster.Name}-http-lb/loadbalancer/servers/2/url", $"https://{newCluster.Ip}:6443"));

            await traefikCache.StoreValues(values);
        }

        private int ExtractBaseId(int valueMaxOrder)
        {
            return valueMaxOrder + 10;
        }

        private int GetBasedRangeIp(Cluster[] existingCluster)
        {
            for (int i = 10; i < 240; i += 10)
            {
                if (!existingCluster.Any(c => c.Ip == $"10.0.{c.ProxmoxNodeId}.{i}"))
                {
                    return i;
                }
            }

            return -1;
        }

        private ClusterCreateMessage GenerateQueueMessage(ClusterCreateRequest request, Cluster newCluster, SshKey selectedSshKey, DatacenterNode selectedNode, Template template, int baseIp, int baseId)
        {

            var message = new ClusterCreateMessage()
            {
                Pattern = "create",
                Data = new Data()
                {
                    Id = baseId,
                    Config = new Config()
                    {
                        User = "root",
                        Password = "homelab0123",
                        Ssh = new Ssh()
                        {
                            PrivateKey = selectedSshKey.Pem,
                            PublicKey = selectedSshKey.Public
                        }
                    },
                    Nodes = new List<Node>()
                }
            };

            var master = new Node()
            {
                Id = baseId,
                CpuCores = 2,
                Master = true,
                Disk = 30,
                Memory = 1024,
                Ip = $"10.0.{selectedNode.Id}.{baseIp}",
                ProxmoxNode = selectedNode.Name,
                Template = template.BaseTemplate
            };

            message.Data.Nodes.Add(master);

            for (var i = 0; i < newCluster.Node; i++)
            {
                var node = new Node()
                {
                    Id = baseId + i + 1,
                    CpuCores = newCluster.Cpu,
                    Master = false,
                    Disk = newCluster.Storage,
                    Memory = newCluster.Memory,
                    Ip = $"10.0.{selectedNode.Id}.{baseIp + i + 1}",
                    ProxmoxNode = selectedNode.Name,
                    Template = template.BaseTemplate
                };

                message.Data.Nodes.Add(node);
            }

            return message;
        }

        public async Task<ClusterItemResponse[]> ListClusterAsync()
        {
            var dbClusters = await clusterRepository.ReadsAsync(c => !c.DeleteAt.HasValue);

            return dbClusters.Select(
                s => new ClusterItemResponse()
                {
                    Id = s.Id,
                    Disk = s.Storage,
                    State = s.State,
                    Cpu = s.Cpu,
                    Memory = s.Memory,
                    Ip = s.Ip,
                    Name = s.Name,
                    Nodes = s.Nodes.Select(n => new ClusterNodeItemResponse()
                    {
                        Id = n.Id,
                        Name = n.Name,
                        Ip = n.Ip,
                        State = n.State
                    }).ToList()
                }).ToArray();
        }

        public Task<(bool found, bool update)> UpdateClusterAsync(Guid id, ClusterUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool found, bool update)> DeleteClusterAsync(Guid id)
        {
            var cluster = await clusterRepository.ReadAsync(c => c.Id == id);

            if (cluster == null)
                return (false, false);

            cluster.DeleteAt = DateTime.UtcNow;

            if ((await clusterRepository.UpdateClusterAsync(cluster)) > 0)
            {
                var nodes = await clusterNodeRepository.ReadsAsync(c => c.ClusterId == cluster.Id && c.DeleteAt == null);
                var selectedNode = await datacenterRepository.ReadAsync(f => f.Id == cluster.ProxmoxNodeId);
                var qemuClient = new QemuClient();

                foreach (var node in nodes)
                {
                    node.DeleteAt = DateTime.UtcNow;
                    await qemuClient.DeleteQemu(selectedNode.Name, node.OrderId);
                    await clusterNodeRepository.UpdateClusterNodeAsync(node);
                }

                return (true, await qemuClient.DeleteQemu(selectedNode.Name, cluster.OrderId));
            }

            return (true, false);
        }


        public async Task<(bool found, bool restart)> RestartClusterMasterAsync(Guid id)
        {
            var cluster = await clusterRepository.ReadAsync(c => c.Id == id && c.DeleteAt == null);
            var selectedNode = await datacenterRepository.ReadAsync(f => f.Id == cluster.ProxmoxNodeId);
            if (cluster == null)
                return (false, false);

            var qemuClient = new QemuClient();

            return (true, await qemuClient.RestartQemu(selectedNode.Name, cluster.OrderId));
        }

        public async Task<(bool found, bool ready, KubeconfigDownloadResponse file)> DownloadKubeconfigAsync(Guid id)
        {
            var cluster = await clusterRepository.ReadAsync(c => c.Id == id);
            if (cluster == null)
                return (false, false, null);

            if (string.IsNullOrEmpty(cluster.KubeConfig))
                return (true, false, null);

            return (true, true, new KubeconfigDownloadResponse()
            {
                Name = $"{cluster.Name}-kubeconfig",
                Content = cluster.KubeConfig
            });
        }
    }
}
