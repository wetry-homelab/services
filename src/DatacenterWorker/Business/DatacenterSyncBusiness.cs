using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProxmoxVEAPI.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Datacenter.Worker.Business
{
    public class DatacenterSyncBusiness : IDatacenterSyncBusiness
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<DatacenterSyncBusiness> logger;
        private readonly IDatacenterRepository datacenterRepository;

        private readonly ClusterClient clusterClient = new ClusterClient();
        private readonly NodeClient nodeClient = new NodeClient();


        public DatacenterSyncBusiness(IConfiguration configuration, ILogger<DatacenterSyncBusiness> logger, IDatacenterRepository datacenterRepository)
        {
            ConfigureClient.Initialise(configuration["Proxmox:Uri"], configuration["Proxmox:Token"]);
            this.configuration = configuration;
            this.logger = logger;
            this.datacenterRepository = datacenterRepository;
        }

        public async Task SynchroniseDatacenter(CancellationToken cancellationToken)
        {
            try
            {
                var proxmoxNodes = await nodeClient.GetNodesAsync();
                var databaseNodes = await datacenterRepository.ReadsAsync();
                var clusterNodes = await clusterClient.GetClusterStatus();

                var updateListTask = new List<Task>();

                foreach (var node in clusterNodes)
                {
                    updateListTask.Add(ProcessNode(proxmoxNodes, databaseNodes, node));
                }

                Task.WaitAll(updateListTask.ToArray());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on processing sync.");
            }
        }

        private async Task ProcessNode(ProxmoxVEAPI.Client.Contracts.Response.Node[] proxmoxNodes, Domain.Entities.DatacenterNode[] databaseNodes, ProxmoxVEAPI.Client.Contracts.Response.ClusterStatus node)
        {
            var selectedNode = databaseNodes.FirstOrDefault(n => n.Name == node.Name && n.Ip == node.Ip);
            var proxmoxNode = proxmoxNodes.FirstOrDefault(n => n.NodeName == node.Name);
            var nodeDetails = await nodeClient.GetNodeStatusAsync(node.Name);

            if (selectedNode != null)
            {
                selectedNode.Name = node.Name;
                selectedNode.Uptime = proxmoxNode.Uptime;

                if ((await datacenterRepository.UpdateDatacenterNodeAsync(selectedNode)) > 0)
                {
                    logger.LogInformation("Existing node update in database.");
                }
            }
            else
            {
                selectedNode = new Domain.Entities.DatacenterNode()
                {
                    Ip = node.Ip,
                    Name = node.Name,
                    Online = node.Online == 1,
                    Uptime = proxmoxNode.Uptime,
                    Core = nodeDetails.CpuInfo.Cores,
                    Thread = nodeDetails.CpuInfo.Cpus,
                    Flag = nodeDetails.CpuInfo.Flags,
                    Mhz = double.Parse(nodeDetails.CpuInfo.Mhz) / 1000,
                    Model = nodeDetails.CpuInfo.Model,
                    RamFree = nodeDetails.Memory.Free,
                    RamTotal = nodeDetails.Memory.Total,
                    RamUsed = nodeDetails.Memory.Used,
                    Socket = nodeDetails.CpuInfo.Sockets,
                    SwapFree = nodeDetails.Swap.Free,
                    SwapTotal = nodeDetails.Swap.Total,
                    SwapUsed = nodeDetails.Swap.Used,
                    KernelVersion = nodeDetails.KernelVersion,
                    PveVersion = nodeDetails.PveVersion,
                    UserHz = nodeDetails.CpuInfo.UserHz,
                    Hvm = nodeDetails.CpuInfo.Hvm == "1",
                    RootFsAvailable = nodeDetails.RootFs.Available,
                    RootFsFree = nodeDetails.RootFs.Free,
                    RootFsTotal = nodeDetails.RootFs.Total,
                    RootFsUsed = nodeDetails.RootFs.Used
                };

                if ((await datacenterRepository.InsertDatacenterNodeAsync(selectedNode)) > 0)
                {
                    logger.LogInformation("New node insert in database.");
                }
            }
        }
    }
}
