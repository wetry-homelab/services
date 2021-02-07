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
    public class ClusterNodeRepository : IClusterNodeRepository
    {
        private readonly ILogger<ClusterNodeRepository> logger;
        private readonly ServiceDbContext serviceDbContext;

        public ClusterNodeRepository(ILogger<ClusterNodeRepository> logger, ServiceDbContext clusterDbContext)
        {
            this.logger = logger;
            this.serviceDbContext = clusterDbContext;
        }

        public Task<ClusterNode> ReadAsync(Expression<Func<ClusterNode, bool>> predicate)
        {
            return serviceDbContext.ClusterNode.FirstOrDefaultAsync(predicate);
        }

        public Task<ClusterNode[]> ReadsAsync(Expression<Func<ClusterNode, bool>> predicate)
        {
            return serviceDbContext.ClusterNode.Where(predicate).ToArrayAsync();
        }

        public Task<int> InsertClusterNodeAsync(ClusterNode entity)
        {
            serviceDbContext.ClusterNode.Add(entity);
            return serviceDbContext.SaveChangesAsync();
        }

        public Task<int> UpdateClusterNodeAsync(ClusterNode entity)
        {
            serviceDbContext.Entry(entity).State = EntityState.Modified;
            return serviceDbContext.SaveChangesAsync();
        }
    }
}
