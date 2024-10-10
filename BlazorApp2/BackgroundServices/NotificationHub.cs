using Microsoft.AspNetCore.SignalR;

namespace BlazorApp2.BackgroundServices
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",message);
        }
    }
}
