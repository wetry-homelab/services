using Application.Interfaces;
using Datacenter.Worker.Business;
using Datacenter.Worker.Workers;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Datacenter.Worker
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
                    webBuilder.UseUrls("http://0.0.0.0:8000", "https://0.0.0.0:8001");
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    services.AddScoped<IDatacenterRepository, DatacenterRepository>();
                    services.AddScoped<IDatacenterSyncBusiness, DatacenterSyncBusiness>();

                    services.AddHostedService<DatacenterSyncWorker>();
                });
    }
}
