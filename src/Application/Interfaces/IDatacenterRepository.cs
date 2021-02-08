using Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDatacenterRepository
    {
        Task<DatacenterNode[]> ReadsAsync();
        Task<DatacenterNode> ReadAsync(Expression<Func<DatacenterNode, bool>> predicate);
        Task<int> InsertDatacenterNodeAsync(DatacenterNode entity);
        Task<int> UpdateDatacenterNodeAsync(DatacenterNode entity);
    }
}
