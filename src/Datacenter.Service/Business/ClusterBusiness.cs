using Application.Interfaces;
using Infrastructure.Contracts.Request;
using Infrastructure.Contracts.Response;
using System;
using System.Threading.Tasks;

namespace Datacenter.Service.Business
{
    public class ClusterBusiness : IClusterBusiness
    {
        public Task<bool> CreateClusterAsync(ClusterCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ClusterItemResponse[]> ListClusterAsync()
        {
            throw new NotImplementedException();
        }

        public Task<(bool found, bool update)> UpdateClusterAsync(Guid id, ClusterUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<(bool found, bool update)> DeleteClusterAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
