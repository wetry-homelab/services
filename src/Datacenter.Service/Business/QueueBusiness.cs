using Application.Interfaces;
using Application.Messages;
using Datacenter.Service.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Threading.Tasks;

namespace Datacenter.Service.Business
{
    public class QueueBusiness : IQueueBusiness
    {
        private readonly IQueueService queueService;
        private readonly IClusterRepository clusterRepository;
        private readonly IHubContext<AppHub, IAppHub> hubContext;

        public QueueBusiness(IQueueService queueService, IHubContext<AppHub, IAppHub> hubContext, IClusterRepository clusterRepository)
        {
            this.queueService = queueService;
            this.hubContext = hubContext;
            this.clusterRepository = clusterRepository;
        }

        public async Task StartQueueListener()
        {
            await queueService.OnQueueMessageInit(async (message) =>
            {
                var clusterResult = JsonSerializer.Deserialize<ClusterCreationResultMessage>(message);
                var cluster = await clusterRepository.ReadAsync(c => c.OrderId == clusterResult.Data.OrderId);

                if (cluster != null)
                {
                    cluster.KubeConfig = clusterResult.Data.KubeConfig;
                    cluster.KubeConfigJson = clusterResult.Data.KubeConfigAsJson;

                    if ((await clusterRepository.UpdateClusterAsync(cluster)) > 0)
                    {
                        await hubContext.Clients.All.NotificationReceived("Cluster ready", "Cluster created and ready to use.", "success");
                    }
                }
            });
        }
    }
}
