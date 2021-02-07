using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class MetricRepository : IMetricRepository
    {
        private readonly ILogger<MetricRepository> logger;
        private readonly ServiceDbContext serviceDbContext;

        public MetricRepository(ILogger<MetricRepository> logger, ServiceDbContext clusterDbContext)
        {
            this.logger = logger;
            this.serviceDbContext = clusterDbContext;
        }

        public Task<int> InsertMetricsAsync(Metric[] metrics)
        {
            serviceDbContext.Metric.AddRange(metrics);
            return serviceDbContext.SaveChangesAsync();
        }
    }
}
