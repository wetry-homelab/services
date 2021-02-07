using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ClusterRepository : IClusterRepository
    {
        private readonly ILogger<ClusterRepository> logger;
        private readonly ServiceDbContext serviceDbContext;

        public ClusterRepository(ILogger<ClusterRepository> logger, ServiceDbContext clusterDbContext)
        {
            this.logger = logger;
            this.serviceDbContext = clusterDbContext;
        }

        public Task<Cluster> ReadAsync(Expression<Func<Cluster, bool>> predicate)
        {
            return serviceDbContext.Cluster.Include(i => i.Nodes).FirstOrDefaultAsync(predicate);
        }

        public Task<Cluster[]> ReadsAsync(Expression<Func<Cluster, bool>> predicate)
        {
            return serviceDbContext.Cluster.Include(i => i.Nodes).Where(predicate).ToArrayAsync();
        }

        public Task<int> InsertClusterAsync(Cluster entity)
        {
            serviceDbContext.Cluster.Add(entity);
            return serviceDbContext.SaveChangesAsync();
        }

        public Task<int> UpdateClusterAsync(Cluster entity)
        {
            serviceDbContext.Entry(entity).State = EntityState.Modified;
            return serviceDbContext.SaveChangesAsync();
        }
    }
}
