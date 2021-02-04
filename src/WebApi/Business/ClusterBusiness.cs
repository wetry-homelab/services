using Application.Contracts.Request;
using Application.Contracts.Response;
using Application.Interfaces;
using Application.Messages;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Business
{
    public class ClusterBusiness : IClusterBusiness
    {
        private readonly ILogger<ClusterBusiness> logger;
        private readonly IClusterRepository clusterRepository;
        private readonly IQueueService queueService;
        private readonly IMapper mapper;

        public ClusterBusiness(ILogger<ClusterBusiness> logger, IClusterRepository clusterRepository, IQueueService queueService, IMapper mapper)
        {
            this.logger = logger;
            this.clusterRepository = clusterRepository;
            this.queueService = queueService;
            this.mapper = mapper;
        }

        public async Task<bool> CreateClusterAsync(ClusterCreateRequest request)
        {
            try
            {
                var newCluster = mapper.Map<Cluster>(request);

                if ((await clusterRepository.InsertClusterAsync(newCluster)) > 0)
                {
                    var messageForCreating = mapper.Map<ClusterCreateMessage>(newCluster);
                    queueService.QueueClusterCreation(messageForCreating);

                    return true;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on creating cluster.");
            }

            return false;
        }

        public async Task<ClusterItemResponse[]> ListClusterAsync()
        {
            try
            {
                var clusters = await clusterRepository.ReadsAsync(c => c.DeleteAt == null);
                return clusters.Select(s => mapper.Map<ClusterItemResponse>(s)).ToArray();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on listing cluster.");
            }

            return null;
        }

        public async Task<(bool found, bool update)> UpdateClusterAsync(Guid id, ClusterUpdateRequest request)
        {
            try
            {
                var cluster = await clusterRepository.ReadAsync(r => r.Id == id);

                if (cluster == null)
                    return (false, false);

                cluster.Cpu = request.Cpu;
                cluster.Memory = request.Memory;
                cluster.Node = request.Node;
                cluster.Storage = request.Storage;
                cluster.State = "Scale";

                if ((await clusterRepository.UpdateClusterAsync(cluster)) > 0)
                {
                    var updateMessage = mapper.Map<ClusterUpdateMessage>(request);
                    queueService.QueueClusterUpdate(updateMessage);
                }

                return (true, true);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on updating cluster.");
            }

            return (true, false);
        }
    }
}
