using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Datacenter.Worker.Workers
{
    public class DatacenterSyncWorker : BackgroundService
    {
        private readonly ILogger<DatacenterSyncWorker> logger;
        public readonly IServiceProvider services;

        public DatacenterSyncWorker(IServiceProvider services,
            ILogger<DatacenterSyncWorker> logger)
        {
            this.services = services;
            this.logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Starting background sync service.");

            await ExecuteSync(stoppingToken);
        }

        private async Task ExecuteSync(CancellationToken stoppingToken)
        {
            do
            {
                var sw = new Stopwatch();
                sw.Start();
                logger.LogInformation("Sync process start.");

                using (var scope = services.CreateScope())
                {
                    var scopedProcessingService =
                        scope.ServiceProvider
                            .GetRequiredService<IDatacenterSyncBusiness>();

                    await scopedProcessingService.SynchroniseDatacenter(stoppingToken);

                    logger.LogInformation($"Sync process ending after {sw.ElapsedMilliseconds} ms. At {DateTime.UtcNow}.");

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
