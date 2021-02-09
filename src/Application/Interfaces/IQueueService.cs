using Application.Messages;
using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IQueueService
    {
        Task OnQueueMessageInit(Action<string> processMessage);
        void QueueClusterCreation(ClusterCreateMessage message);
        void QueueClusterUpdate(ClusterUpdateMessage message);
    }
}
