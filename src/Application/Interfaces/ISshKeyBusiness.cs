using Infrastructure.Contracts.Request;
using Infrastructure.Contracts.Response;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISshKeyBusiness
    {
        Task<bool> InsertKeyAsync(SshKeyCreateRequest request);
        Task<SshKeyResponse[]> ReadKeysAsync();
        Task<bool> DeleteKeyAsync(int id);
        Task<SshKeyDownloadResponse> DownloadAsync(int id, string type);
    }
}
