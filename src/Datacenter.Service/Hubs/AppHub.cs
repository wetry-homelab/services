using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Datacenter.Service.Hubs
{
    public class AppHub : Hub<IAppHub>
    {
        public async Task NotifiyUserAsync(string title, string content, string type)
        {
            await Clients.All.NotificationReceived(title, content, type);
        }
    }
}
