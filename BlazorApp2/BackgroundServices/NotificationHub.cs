using BlazorApp2.Services.Jobs;
using Microsoft.AspNetCore.SignalR;

namespace BlazorApp2.BackgroundServices;

public class JobHub : Hub
{
    public async Task SendJobUpdate(string message)
    {
        await Clients.All.SendAsync("ReceiveJobUpdate", message);
    }
}
