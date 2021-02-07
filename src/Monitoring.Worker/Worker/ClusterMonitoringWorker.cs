using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Monitoring.Worker.Worker
{
    public class ClusterMonitoringWorker : BackgroundService
    {
        private readonly ILogger<ClusterMonitoringWorker> logger;
        public readonly IServiceProvider services;

        public ClusterMonitoringWorker(IServiceProvider services,
            ILogger<ClusterMonitoringWorker> logger)
        {
            this.services = services;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Starting background monitoring service.");
            await ExecuteSync(stoppingToken);
        }

        private async Task ExecuteSync(CancellationToken stoppingToken)
        {
            do
            {
                using (var scope = services.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                            .GetRequiredService<IClusterMonitoringBusiness>();

                    await scopedProcessingService.MonitorClustersAsync(stoppingToken);

                    await Task.Delay(30000);
                }
            } while (!stoppingToken.IsCancellationRequested);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Stopping sync background service.");
            await base.StopAsync(stoppingToken);
        }
    }
}
