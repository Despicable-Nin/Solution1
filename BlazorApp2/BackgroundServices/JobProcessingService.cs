﻿using BlazorApp2.Services.Jobs;
using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using BlazorApp2.Services.Enumerations;
using Serilog;
using System.Collections.Concurrent;
using BlazorApp2.Services.Geocoding;
using Microsoft.AspNetCore.SignalR;

namespace BlazorApp2.BackgroundServices;

public class JobProcessingService(
    IAddressRepository addressRepository,
    ICrimeRepository crimeRepository,
    IEnumeration enumService,
    IJobService jobService,
    AddressProcessorService geocodingService,
    IHubContext<NotificationHub> hubContext)
{
    public async Task ProcessJobAsync()
    {
        Log.Logger.Information($"Started {nameof(JobProcessingService.ProcessJobAsync)}");


        try
        {

            Dictionary<string, (double Latitude, double Longitude)> addressBook = await addressRepository.GetAddresses() ?? [];
            var crimes = await crimeRepository.GetUnsanitizedCrimeRecords();
            if (crimes.Any())
            {
                await PersistEnumerationsAsync(crimes);

                var crimeMotives = await enumService.GetCrimeMotives();
                var crimeTypes = await enumService.GetCrimeTypes();
                var severities = await enumService.GetSeverities();
                var precincts = await enumService.GetPoliceDistricts();
                var weathers = await enumService.GetWeatherConditions();

             
                foreach (var crime in crimes)
                {
                    (string SanitizedAddress, (double? Latitude, double? Longitude)) result = default;

                    var crimeAddress = crime.Address.ToUpper();
                    //foreach(var key in addressBook.Keys)
                    //{
                    //    if(key.ToUpper().Contains(crimeAddress) )
                    //    {
                    //        result = (key.ToUpper(), addressBook[key]);
                    //    }
                    //}

                    if (result == default)
                    {
                        if (addressBook.ContainsKey(crimeAddress!))
                        {
                            var latLong = addressBook!.GetValueOrDefault(crimeAddress);
                            result = (crimeAddress, latLong);
                        }
                        else
                        {
                            Log.Logger.Information("Fetching GIS -- limiting rate by 1 request / second.");
                            result = await geocodingService.GetLatLongAsync(crimeAddress!);
                            var isNewAddress = addressBook.TryAdd(result.SanitizedAddress!.ToUpper(), (result.Item2.Latitude!.Value, result.Item2.Longitude!.Value));

                            if (isNewAddress)
                            {
                                await addressRepository.CreateAddress(
                                  new Address
                                  {
                                      Description = result.SanitizedAddress!.ToUpper(),
                                      Latitude = result.Item2.Latitude,
                                      Longitude = result.Item2.Longitude,
                                      Id = Guid.NewGuid()
                                  });

                                await addressRepository.SaveChangesAsync(CancellationToken.None);
                            }
                        }
                    }

                    //hydrate fields
                    crime.SanitizedAddress = result.SanitizedAddress;
                    crime.Longitude = (float)result.Item2.Longitude.GetValueOrDefault();
                    crime.Latitude = (float)result.Item2.Latitude.GetValueOrDefault();
                    crime.PoliceDistrictId = precincts.SingleOrDefault(i => i.Value == crime.PoliceDistrict).Key.ToString() ?? "0";
                    crime.SeverityId = severities.SingleOrDefault(i => i.Value == crime.Severity).Key.ToString() ?? "0";
                    crime.WeatherConditionId = weathers.SingleOrDefault(i => i.Value == crime.WeatherCondition).Key.ToString() ?? "0";
                    crime.CrimeMotiveId = crimeMotives.SingleOrDefault(i => i.Value == crime.CrimeMotive).Key.ToString() ?? "0";
                    crime.CrimeTypeId = crimeTypes.SingleOrDefault(i => i.Value == crime.CrimeType).Key.ToString() ?? "0";

                    await crimeRepository.UpdateCrimeAsync(crime);
                }

                await crimeRepository.SaveChangesAsync(CancellationToken.None);

                Log.Logger.Information("Successfully processed ...");
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex, "Something went wrong: {Message}", ex.Message);
            await hubContext.Clients.All.SendAsync("ReceiveJobUpdate", ex.Message);
            return;
        }
           
        await hubContext.Clients.All.SendAsync("ReceiveJobUpdate", "Finished sanitizing data.");
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
