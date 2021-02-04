using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class DatacenterRepository : IDatacenterRepository
    {
        private readonly ServiceDbContext serviceDbContext;

        public DatacenterRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public Task<DatacenterNode[]> ReadsAsync()
        {
            return serviceDbContext.DatacenterNode.ToArrayAsync();
        }

        public Task<int> InsertDatacenterNodeAsync(DatacenterNode entity)
        {
            serviceDbContext.DatacenterNode.Add(entity);
            return serviceDbContext.SaveChangesAsync();
        }

        public Task<int> UpdateDatacenterNodeAsync(DatacenterNode entity)
        {
            serviceDbContext.Entry(entity).State = EntityState.Modified;
            return serviceDbContext.SaveChangesAsync();
        }
    }
}
