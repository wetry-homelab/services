using Application.Interfaces;
using Domain.Entities;
using k8s;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Monitoring.Worker.Business
{
    public class ClusterMonitoringBusiness : IClusterMonitoringBusiness
    {
        private readonly IClusterRepository clusterRepository;
        private readonly IClusterNodeRepository clusterNodeRepository;
        private readonly IMetricRepository metricRepository;
        private readonly ILogger<ClusterMonitoringBusiness> logger;

        public ClusterMonitoringBusiness(IClusterRepository clusterRepository, ILogger<ClusterMonitoringBusiness> logger, IClusterNodeRepository clusterNodeRepository, IMetricRepository metricRepository)
        {
            this.clusterRepository = clusterRepository;
            this.logger = logger;
            this.clusterNodeRepository = clusterNodeRepository;
            this.metricRepository = metricRepository;
        }

        public async Task MonitorClustersAsync(CancellationToken cancellationToken)
        {
            if (!Directory.Exists("./configs"))
                Directory.CreateDirectory("./configs");

            var clusters = await clusterRepository.ReadsAsync(c => !string.IsNullOrEmpty(c.KubeConfigJson) && c.DeleteAt == null);
            var clusterTask = new List<Task>();

            foreach (var cls in clusters)
            {
                clusterTask.Add(MonitorClusterAsync(cls));
            }

            Task.WaitAll(clusterTask.ToArray());
        }

        private async Task MonitorClusterAsync(Domain.Entities.Cluster cluster)
        {
            try
            {
                var filePath = $"./configs/{cluster.Id}/kubeconfig";

                if (!File.Exists(filePath))
                {
                    await File.WriteAllTextAsync(filePath, cluster.KubeConfig);
                }

                var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(filePath);
                var client = new Kubernetes(config);

                var clusterNodes = await client.ListNodeAsync();

                Task.WaitAll(new Task[] { ProcessStateAsync(cluster, clusterNodes), ProcessMetricsAsync(cluster, client) });
            }
            catch (Exception e)
            {
                logger.LogError(e, "Fail to connect client.");
            }
        }

        private async Task ProcessStateAsync(Domain.Entities.Cluster cluster, k8s.Models.V1NodeList clusterNodes)
        {
            try
            {
                foreach (var clusterNode in clusterNodes.Items)
                {
                    if (clusterNode.Metadata.Name.Contains("master"))
                    {
                        cluster.State = clusterNode.Status.Conditions.FirstOrDefault(c => c.Reason == "KubeletReady")?.Type;
                        var _ = await clusterRepository.UpdateClusterAsync(cluster);
                    }
                    else
                    {
                        var node = cluster.Nodes.FirstOrDefault(n => n.Name == clusterNode.Metadata.Name);
                        if (node != null)
                        {
                            node.State = clusterNode.Status.Conditions.FirstOrDefault(c => c.Reason == "KubeletReady")?.Type;
                            var __ = await clusterNodeRepository.UpdateClusterNodeAsync(node);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on processing state");
            }
        }

        private async Task ProcessMetricsAsync(Domain.Entities.Cluster cluster, Kubernetes client)
        {
            try
            {
                var metrics = await client.GetKubernetesNodesMetricsAsync();
                var metricsGathered = new List<Metric>();

                foreach (var metric in metrics.Items)
                {
                    var node = cluster.Nodes.FirstOrDefault(n => n.Name == metric.Metadata.Name);
                    var extractItemId = new Guid();

                    if (node != null)
                        extractItemId = node.Id;
                    else if (metric.Metadata.Name.Contains("master"))
                        extractItemId = cluster.Id;

                    if (extractItemId != Guid.Empty)
                    {
                        metricsGathered.Add(new Metric()
                        {
                            EntityId = extractItemId,
                            CpuValue = long.Parse(metric.Usage["cpu"].CanonicalizeString().Replace("n", "")),
                            MemoryValue = long.Parse(metric.Usage["memory"].CanonicalizeString().Replace("Ki", ""))
                        });

                    }
                }

                var _ = await metricRepository.InsertMetricsAsync(metricsGathered.ToArray());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on processing state");
            }
        }
    }
}
