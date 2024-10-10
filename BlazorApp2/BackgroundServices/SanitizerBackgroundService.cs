
using Serilog;

namespace BlazorApp2.BackgroundServices;

public class SanitizerBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        try
        {
            // Your background task logic here
            await Task.Delay(5000, stoppingToken); // Example: Simulate a 5-second task
            Log.Logger.Information("Background task completed successfully.");
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "An error occurred in the background task.");
        }
        finally
        {
            // Ensure the service stops after completion
            stoppingToken.ThrowIfCancellationRequested();
        }
    }
}
