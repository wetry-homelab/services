using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class SshKeyRepository : ISshKeyRepository
    {
        private readonly ServiceDbContext serviceDbContext;

        public SshKeyRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public Task<SshKey[]> ReadsAsync()
        {
            return serviceDbContext.SshKey.Where(s => !s.DeleteAt.HasValue).ToArrayAsync();
        }

        public Task<SshKey> ReadAsync(int id)
        {
            return serviceDbContext.SshKey.FirstOrDefaultAsync(s => !s.DeleteAt.HasValue && s.Id == id);
        }

        public Task<int> InsertAsync(SshKey entity)
        {
            serviceDbContext.SshKey.Add(entity);
            return serviceDbContext.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(SshKey sshKey)
        {
            serviceDbContext.Entry(sshKey).State = EntityState.Modified;
            return serviceDbContext.SaveChangesAsync();
        }
    }
}
