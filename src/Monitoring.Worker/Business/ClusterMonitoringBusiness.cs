using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Monitoring.Worker.Business
{
    public class ClusterMonitoringBusiness : IClusterMonitoringBusiness
    {
        private readonly IClusterRepository clusterRepository;
        private readonly ILogger<ClusterMonitoringBusiness> logger;

        public ClusterMonitoringBusiness(IClusterRepository clusterRepository, ILogger<ClusterMonitoringBusiness> logger)
        {
            this.clusterRepository = clusterRepository;
            this.logger = logger;
        }
    }
}
