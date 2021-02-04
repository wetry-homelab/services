using Application.Contracts.Request;
using Application.Contracts.Response;
using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClusterBusiness
    {
        Task<ClusterItemResponse[]> ListClusterAsync();
        Task<bool> CreateClusterAsync(ClusterCreateRequest request);
        Task<(bool found, bool update)> UpdateClusterAsync(Guid id, ClusterUpdateRequest request);
    }
}
