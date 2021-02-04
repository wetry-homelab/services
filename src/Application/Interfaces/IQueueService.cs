using Application.Messages;

namespace Application.Interfaces
{
    public interface IQueueService
    {
        void QueueClusterCreation(ClusterCreateMessage message);
        void QueueClusterUpdate(ClusterUpdateMessage message);
    }
}
