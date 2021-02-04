using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISshKeyRepository
    {
        Task<SshKey[]> ReadsAsync();
        Task<SshKey> ReadAsync(int id);
        Task<int> InsertAsync(SshKey entity);
        Task<int> UpdateAsync(SshKey sshKey);
    }
}
