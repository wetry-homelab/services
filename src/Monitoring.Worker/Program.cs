using Application.Interfaces;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monitoring.Worker.Business;
using Monitoring.Worker.Worker;

namespace Monitoring.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureServices(services =>
                {
                    services.AddScoped<IClusterRepository, ClusterRepository>();
                    services.AddScoped<IClusterMonitoringBusiness, ClusterMonitoringBusiness>();

                    services.AddHostedService<ClusterMonitoringWorker>();
                });
    }
}
