using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAppHub
    {
        Task NotificationReceived(string title, string content, string type);
    }
}
