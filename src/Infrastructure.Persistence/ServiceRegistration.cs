using Application.Interfaces;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SQL_CONNECTION")))
            {
                connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION");
            }

            services.AddDbContext<ServiceDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(10);
                });
            }, ServiceLifetime.Scoped);

            services.AddScoped<IClusterRepository, ClusterRepository>();
            services.AddScoped<ISshKeyRepository, SshKeyRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<IDatacenterRepository, DatacenterRepository>();
        }
    }
}
