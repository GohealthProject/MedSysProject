using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MedSysProject.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string userId,string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", userId, user, message);
        }
    }
}