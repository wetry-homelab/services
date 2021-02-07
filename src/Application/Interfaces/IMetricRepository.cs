using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMetricRepository
    {
        Task<int> InsertMetricsAsync(Metric[] metrics);
        Task InsertMetricsWithStrategyAsync(Metric[] metrics);
    }
}
