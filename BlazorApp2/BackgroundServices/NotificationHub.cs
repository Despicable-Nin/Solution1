using BlazorApp2.Services.Jobs;
using Microsoft.AspNetCore.SignalR;

namespace BlazorApp2.BackgroundServices;

public class NotificationHub : Hub
{
    public async Task SendJobUpdate(string message)
    {
        await Clients.All.SendAsync("ReceiveJobUpdate", message);
    }

    public async Task Test(string message)
    {
        await Clients.All.SendAsync("Test", message);
    }
}
