using BlazorApp2.Services.Jobs;
using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using BlazorApp2.Services.Enumerations;
using Serilog;
using System.Collections.Concurrent;
using BlazorApp2.Services.Geocoding;
using Microsoft.AspNetCore.SignalR;

namespace BlazorApp2.BackgroundServices;

public class JobProcessingService(
    ICrimeRepository crimeRepository,
    IEnumeration enumService,
    IJobService jobService,
    NominatimGeocodingService geocodingService,
    IHubContext<NotificationHub> hubContext)
{
    public async Task ProcessJobAsync()
    {
        Log.Logger.Information($"Started {nameof(JobProcessingService.ProcessJobAsync)}");

        JobDto? jobDto = await jobService.GetAJob();

        if (jobDto == null)
        {
            Log.Logger.Information("Job pool empty.");
            return;
        }

        if (!Guid.TryParse(jobDto.Name, out var batchId))
        {
            throw new InvalidOperationException($"Job {jobDto} has an invalid name. Should be of type Guid");
        }

        try
        {

            if (jobDto.Status == JobStatus.Created)
            {
                await jobService.UpdateJob(jobDto with { Status = JobStatus.Running });
            }

            var dto = await crimeRepository.GetCrimesByBatchIdAsync(batchId);
            var crimes = dto.Where(i => i.Latitude == null || i.Longitude == null).ToArray();
            if (crimes.Length != 0)
            {
                await PersistEnumerationsAsync(crimes);

                var crimeMotives = await enumService.GetCrimeMotives();
                var crimeTypes = await enumService.GetCrimeTypes();
                var severities = await enumService.GetSeverities();
                var precincts = await enumService.GetPoliceDistricts();
                var weathers = await enumService.GetWeatherConditions();

                Dictionary<string, (double Latitude, double Longitude)> addressBook = [];
                foreach (var crime in crimes)
                {
                    (double Latitude, double Longitude) result;
                    if (addressBook.ContainsKey(crime.Address!))
                    {
                        result = addressBook!.GetValueOrDefault(crime.Address);
                    }
                    else
                    {
                        result = await geocodingService.GetLatLongAsync(crime.Address!);
                        addressBook.TryAdd(crime.Address!, (result.Latitude, result.Longitude));

                        Log.Logger.Information("Fetching GIS -- limiting rate by 1 request / second.");
                        await Task.Delay(1000);
                    }

                    //hydrate fields
                    crime.Longitude = (float)result.Longitude;
                    crime.Latitude = (float)result.Latitude;
                    crime.PoliceDistrictId = precincts.SingleOrDefault(i => i.Value == crime.PoliceDistrict).Key.ToString() ?? "0";
                    crime.SeverityId = severities.SingleOrDefault(i => i.Value == crime.Severity).Key.ToString() ?? "0";
                    crime.WeatherConditionId = weathers.SingleOrDefault(i => i.Value == crime.WeatherCondition).Key.ToString() ?? "0";
                    crime.CrimeMotiveId = crimeMotives.SingleOrDefault(i => i.Value == crime.CrimeMotive).Key.ToString() ?? "0";
                    crime.CrimeTypeId = crimeTypes.SingleOrDefault(i => i.Value == crime.CrimeType).Key.ToString() ?? "0";

                    await crimeRepository.UpdateCrimeAsync(crime);
                }

                await crimeRepository.SaveChangesAsync(CancellationToken.None);

                jobDto = jobDto with { Status = JobStatus.Suceeded };
                await jobService.UpdateJob(jobDto);

                Log.Logger.Information("Successfully processed ...");
            }

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
            await hubContext.Clients.All.SendAsync("ReceiveJobUpdate", "Finished sanitizing data.");
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
