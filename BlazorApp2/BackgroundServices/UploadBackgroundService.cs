
using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using BlazorApp2.Services.Crimes;
using BlazorApp2.Services.Enumerations;
using BlazorApp2.Services.Jobs;
using Serilog;

namespace BlazorApp2.BackgroundServices;

public class UploadBackgroundService(IServiceScopeFactory serviceScopeFactory) : IHostedService, IDisposable
{

    private Timer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        Log.Logger.Information("Starting {ServiceName} - {Timestamp}", nameof(UploadBackgroundService), DateTime.Now);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Log.Logger.Information("Stopping {ServiceName} - {Timestamp}", nameof(UploadBackgroundService), DateTime.Now);
        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        // Fetch GIS data
        Log.Logger.Information("This is logged from {ServiceName} - {Timestamp}", nameof(UploadBackgroundService), DateTime.Now);
        
        using var scope = serviceScopeFactory.CreateScope();
        var crimeRepository = scope.ServiceProvider.GetRequiredService<ICrimeRepository>();
        var enumService = scope.ServiceProvider.GetRequiredService<IEnumeration>();
        var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();

        var jobDto = await jobService.GetAJob();

        if (jobDto == null) return;

        if (!Guid.TryParse(jobDto.Name, out var batchId)) {
            throw new InvalidOperationException($"Job {jobDto} has an invalid name. SHould be of type Guid");
        }

        Log.Logger.Information("Job fetched: {Job}", jobDto);
        if(jobDto.Status == JobStatus.Created)
        {
            try
            {
                jobDto = jobDto with { Status = JobStatus.Running };
                await jobService.UpdateJob(jobDto);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to update job : {Job}. Reason: {Reason}", jobDto, ex.Message + ". " + ex.InnerException?.Message);
            }
            
        }

        try
        {
            var result = await crimeRepository.GetCrimesByBatchIdAsync(batchId);
            if (result == null || !result.Any())
            {
                try
                {
                    jobDto = jobDto with { Status = JobStatus.Running };
                    await jobService.UpdateJob(jobDto);

                    await PersistEnumerationsAsync(enumService, result!);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "Failed to update job : {Job}. Reason: {Reason}", jobDto, ex.Message + ". " + ex.InnerException?.Message);
                    Log.Logger.Error(ex, "Data processing failed {DateTimeNow}...", DateTime.Now);
                }  
            }

        }catch(Exception ex)
        {
            Log.Logger.Error(ex, "Failed to update job : {Job}. Reason: {Reason}", jobDto, ex.Message + ". " + ex.InnerException?.Message);
        }
    }

    private static async Task PersistEnumerationsAsync(IEnumeration enumService, IEnumerable<Crime>? crimes)
    {
        crimes ??= [];
        try
        {
            // add crimeTypes before saving
            await enumService.AddCrimeTypes(crimes.Select(c => c.CrimeType).Distinct()!);
        }
        catch (Exception ex)
        {
            Log.Logger.Warning(ex, "Persisting '{Entity}' failed. Reason: {Reason} {AdditionalInfo}", nameof(CrimeType), ex.Message, ex.InnerException?.Message);
        }

        try
        {
            // add crimeMotive before saving
            await enumService.AddCrimeMotives(crimes.Select(c => c.CrimeMotive).Distinct()!);
        }
        catch (Exception ex)
        {
            Log.Logger.Warning(ex, "Persisting '{Entity}' failed. Reason: {Reason} {AdditionalInfo}", nameof(CrimeMotive), ex.Message, ex.InnerException?.Message);
        }

        try
        {
            // add weatherConditions before saving
            await enumService.AddWeatherConditions(crimes.Select(c => c.WeatherCondition).Distinct()!);

        }
        catch (Exception ex)
        {
            Log.Logger.Warning(ex, "Persisting '{Entity}' failed. Reason: {Reason} {AdditionalInfo}", nameof(Weather), ex.Message, ex.InnerException?.Message);
        }

        try
        {

            // add policeDistricts before saving
            await enumService.AddPoliceDistricts(crimes.Select(c => c.PoliceDistrict).Distinct()!);
        }
        catch (Exception ex)
        {
            Log.Logger.Warning(ex, "Persisting '{Entity}' failed. Reason: {Reason} {AdditionalInfo}", nameof(PoliceDistrict), ex.Message, ex.InnerException?.Message);
        }

        try
        {

            // add severityLevels before saving
            await enumService.AddSeverities(crimes.Select(c => c.Severity).Distinct()!);

        }
        catch (Exception ex)
        {
            Log.Logger.Warning(ex, "Persisting '{Entity}' failed. Reason: {Reason} {AdditionalInfo}", nameof(Severity), ex.Message, ex.InnerException?.Message);
        }
    }


    #region IDisposable
    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
