using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDatacenterSyncBusiness
    {
        Task SynchroniseDatacenter(CancellationToken cancellationToken);
    }
}
