using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDatacenterRepository
    {
        Task<DatacenterNode[]> ReadsAsync();


        Task<int> InsertDatacenterNodeAsync(DatacenterNode entity);

        Task<int> UpdateDatacenterNodeAsync(DatacenterNode entity);
    }
}
