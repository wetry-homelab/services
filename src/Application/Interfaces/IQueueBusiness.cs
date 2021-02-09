using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IQueueBusiness
    {
        Task StartQueueListener();
    }
}
