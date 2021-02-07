using Application.Interfaces;
using Domain.Entities;
using k8s;
using k8s.KubeConfigModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            var clusters = await clusterRepository.ReadsAsync(c => !string.IsNullOrEmpty(c.KubeConfig) && c.DeleteAt == null);
            var clusterTask = new List<Task>();


            foreach (var cls in clusters)
            {
                clusterTask.Add(MonitorClusterAsync(cls));
            }

            Task.WaitAll(clusterTask.ToArray());
        }

        private async Task MonitorClusterAsync(Domain.Entities.Cluster cluster)
        {
            var kubeconfig = JsonSerializer.Deserialize<K8SConfiguration>(cluster.KubeConfigJson);
            var config = KubernetesClientConfiguration.BuildConfigFromConfigObject(kubeconfig);
            var client = new Kubernetes(config);

            var clusterNodes = await client.ListNodeAsync();

            Task.WaitAll(new Task[] { ProcessStateAsync(cluster, clusterNodes), ProcessMetricsAsync(cluster, client) });
        }

        private async Task ProcessStateAsync(Domain.Entities.Cluster cluster, k8s.Models.V1NodeList clusterNodes)
        {
            try
            {
                foreach (var clusterNode in clusterNodes.Items)
                {
                    if (clusterNode.Metadata.Name.Contains("master"))
                    {
                        cluster.State = clusterNode.Status.Conditions.FirstOrDefault(c => c.Reason == "KubeletReady")?.Status;
                        var _ = await clusterRepository.UpdateClusterAsync(cluster);
                    }
                    else
                    {
                        var node = cluster.Nodes.FirstOrDefault(n => n.Name == clusterNode.Metadata.Name);
                        if (node != null)
                        {
                            node.State = clusterNode.Status.Conditions.FirstOrDefault(c => c.Reason == "KubeletReady")?.Status;
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

                foreach (var metric in metrics.Items)
                {
                    var node = cluster.Nodes.FirstOrDefault(n => n.Name == metric.Metadata.Name);
                    var extractItemId = new Guid();

                    if (node != null)
                        extractItemId = node.Id;
                    else
                        extractItemId = node.ClusterId;

                    if (extractItemId != Guid.Empty)
                    {
                        var metricsGathered = new List<Metric>(){
                            new Metric()
                            {
                                ItemId = node.Id,
                                Type = "cpu",
                                Value = float.Parse(metric.Usage["cpu"].CanonicalizeString())
                            }, new Metric()
                            {
                                ItemId = node.Id,
                                Type = "memory",
                                Value = float.Parse(metric.Usage["memory"].CanonicalizeString())
                            }
                        };

                        var _ = await metricRepository.InsertMetricsAsync(metricsGathered.ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on processing state");
            }
        }
    }
}
