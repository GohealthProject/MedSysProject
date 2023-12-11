using Microsoft.AspNetCore.SignalR;

namespace MedSysProject.Hubs
{
    public class ChatHub
    {
        public async Task SendMessage(string user, string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
