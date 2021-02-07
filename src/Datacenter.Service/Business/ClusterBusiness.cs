using Application.Interfaces;
using Infrastructure.Contracts.Request;
using Infrastructure.Contracts.Response;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Datacenter.Service.Business
{
    public class ClusterBusiness : IClusterBusiness
    {
        private readonly IClusterRepository clusterRepository;

        public ClusterBusiness(IClusterRepository clusterRepository)
        {
            this.clusterRepository = clusterRepository;
        }

        public Task<bool> CreateClusterAsync(ClusterCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ClusterItemResponse[]> ListClusterAsync()
        {
            var dbClusters = await clusterRepository.ReadsAsync(c => !c.DeleteAt.HasValue);

            return dbClusters.Select(
                s => new ClusterItemResponse()
                {
                    Id = s.Id,
                    Disk = s.Storage,
                    State = s.State,
                    Cpu = s.Cpu,
                    Memory = s.Memory,
                    Ip = s.Ip,
                    Name = s.Name,
                    Nodes = s.Nodes.Select(n => new ClusterNodeItemResponse()
                    {
                        Id = n.Id,
                        Name = n.Name,
                        Ip = n.Ip,
                        State = n.State
                    }).ToList()
                }).ToArray();
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
