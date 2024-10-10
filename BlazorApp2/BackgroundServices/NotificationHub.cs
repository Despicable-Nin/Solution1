using BlazorApp2.Services.Jobs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using BlazorApp2.Services.Enumerations;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Concurrent;
using BlazorApp2.Services.Geocoding;

namespace BlazorApp2.BackgroundServices;

public class JobHub : Hub
{
    public async Task SendJobUpdate(JobDto job)
    {
        await Clients.All.SendAsync("ReceiveJobUpdate", job);
    }
}



public class JobConsumerService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHubContext<JobHub> _hubContext;

    public JobConsumerService(IServiceScopeFactory serviceScopeFactory, IHubContext<JobHub> hubContext)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Your job processing logic here...

        // Example of sending a job update to the clients
        var jobDto = new JobDto { /* Initialize jobDto */ };
        await _hubContext.Clients.All.SendAsync("ReceiveJobUpdate", jobDto);
    }
}



public class JobProcessingService(ICrimeRepository crimeRepository, IEnumeration enumService, IJobService jobService, NominatimGeocodingService geocodingService)
{
    public async Task ProcessJobAsync()
    {
        JobDto? jobDto = await jobService.GetAJob();

        if (jobDto == null)
        {
            Log.Logger.Information("Job pool empty.");
            return;
        }

        Log.Logger.Information("Job fetched: {Job}", jobDto);

        Log.Information("Starting job processing for {JobId}", jobDto.Id);

        if (!Guid.TryParse(jobDto.Name, out var batchId))
        {
            Log.Error("Invalid job name: {JobName}", jobDto.Name);
            throw new InvalidOperationException($"Job {jobDto} has an invalid name. Should be of type Guid");
        }

        Log.Information("Job fetched: {Job}", jobDto);
        if (jobDto.Status == JobStatus.Created)
        {
            try
            {
                jobDto = jobDto with { Status = JobStatus.Running };
                await jobService.UpdateJob(jobDto);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to update job: {Job}", jobDto);
            }
        }

        try
        {
            var dto = await crimeRepository.GetCrimesByBatchIdAsync(batchId);
            var crimes = dto.Where(i => i.Latitude == null || i.Longitude == null).ToArray();
            if (crimes.Any())
            {
               await PersistEnumerationsAsync(crimes);
            }

            var addressBook = new ConcurrentDictionary<string, (double Latitude, double Longitude)>();
            foreach (var crime in crimes)
            {
                (double Latitude, double Longitude) result = (0F, 0F);
                if (addressBook.ContainsKey(crime.Address))
                {
                    result = addressBook.GetValueOrDefault(crime.Address);
                }
                else
                {
                    result = await geocodingService.GetLatLongAsync(crime.Address);
                    addressBook.TryAdd(crime.Address, (result.Latitude, result.Longitude));
                    Log.Logger.Information("Fetching GIS -- limiting rate by 1 request / second.");
                    await Task.Delay(1000);
                }

                crime.Longitude = (float)result.Longitude;
                crime.Latitude = (float)result.Latitude;

                await crimeRepository.UpdateCrimeAsync(crime);
            }

            await crimeRepository.SaveChangesAsync(CancellationToken.None);

            Log.Logger.Information("Successfully processed ...");

        }
        catch (Exception ex)
        {
            Log.Error(ex, "Something went wrong: {Job}", jobDto);
            try
            {
                jobDto = jobDto with { Status = JobStatus.Failed };
                await jobService.UpdateJob(jobDto);
            }
            catch (Exception ee)
            {
                Log.Error(ee, "You failed at failing. hahaha...", jobDto);
            }
            return;
        }
        finally
        {
            jobDto = jobDto with { Status = JobStatus.Suceeded };
            await jobService.UpdateJob(jobDto);
        }
    }

    private async Task PersistEnumerationsAsync(IEnumerable<Crime> crimes)
    {
        try
        {
            await enumService.AddCrimeTypes(crimes.Select(c => c.CrimeType).Distinct()!);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Persisting '{Entity}' failed. Reason: {Reason}", nameof(CrimeType), ex.Message);
        }

        try
        {
            await enumService.AddCrimeMotives(crimes.Select(c => c.CrimeMotive).Distinct()!);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Persisting '{Entity}' failed. Reason: {Reason}", nameof(CrimeMotive), ex.Message);
        }

        try
        {
            await enumService.AddWeatherConditions(crimes.Select(c => c.WeatherCondition).Distinct()!);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Persisting '{Entity}' failed. Reason: {Reason}", nameof(Weather), ex.Message);
        }

        try
        {
            await enumService.AddPoliceDistricts(crimes.Select(c => c.PoliceDistrict).Distinct()!);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Persisting '{Entity}' failed. Reason: {Reason}", nameof(PoliceDistrict), ex.Message);
        }

        try
        {
            await enumService.AddSeverities(crimes.Select(c => c.Severity).Distinct()!);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Persisting '{Entity}' failed. Reason: {Reason}", nameof(Severity), ex.Message);
        }
    }
}
