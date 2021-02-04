using Infrastructure.Contracts.Response;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDatacenterBusiness
    {
        Task<DatacenterNodeResponse[]> GetDatacenter();
    }
}
