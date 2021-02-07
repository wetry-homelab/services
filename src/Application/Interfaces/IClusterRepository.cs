using Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClusterRepository
    {
        Task<int> InsertClusterAsync(Cluster entity);
        Task<int> UpdateClusterAsync(Cluster entity);
        Task<Cluster> ReadAsync(Expression<Func<Cluster, bool>> predicate);
        Task<Cluster[]> ReadsAsync(Expression<Func<Cluster, bool>> predicate);
        Task<int> GetMaxOrder();
    }
}
