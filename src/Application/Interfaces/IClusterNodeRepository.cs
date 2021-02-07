using Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClusterNodeRepository
    {
        Task<ClusterNode[]> ReadsAsync(Expression<Func<ClusterNode, bool>> predicate);
        Task<int> InsertClusterNodeAsync(ClusterNode entity);
        Task<int> UpdateClusterNodeAsync(ClusterNode entity);
        Task<ClusterNode> ReadAsync(Expression<Func<ClusterNode, bool>> predicate);
    }
}
