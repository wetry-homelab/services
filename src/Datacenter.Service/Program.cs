using Application.Interfaces;
using Datacenter.Service.Business;
using Datacenter.Service.Workers;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Shared.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Datacenter.Service
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
                }).ConfigureServices((services) =>
                {
                    services.AddSignalR();

                    services.AddScoped<IClusterRepository, ClusterRepository>();
                    services.AddScoped<IQueueService, QueueService>();
                    services.AddScoped<IQueueBusiness, QueueBusiness>();

                    //services.AddHostedService<ClusterQueueWorker>();
                });
    }
}
