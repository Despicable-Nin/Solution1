using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace BlazorApp2.BackgroundServices;

public class SanitizerBackgroundService(IHubContext<NotificationHub> hubContext)
{
    public async Task RunSanitizationTask(CancellationToken stoppingToken)
    {
        try
        {
            // Your background task logic here
            await Task.Delay(5000, stoppingToken); // Example: Simulate a 5-second task
            Log.Logger.Information("Background task completed successfully.");

            // Notify clients
            await hubContext.Clients.All.SendAsync("ReceiveMessage", "Sanitization task completed successfully.");
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "An error occurred in the background task.");
        }
    }
}