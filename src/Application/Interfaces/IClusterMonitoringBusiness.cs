using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClusterMonitoringBusiness
    {
        Task MonitorClustersAsync(CancellationToken cancellationToken);
    }
}
