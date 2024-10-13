﻿using BlazorApp2.Repositories.Interfaces;
using BlazorApp2.Repositories;
using BlazorApp2.Services.Clustering;
using BlazorApp2.Services.Crimes;
using BlazorApp2.Services.Enumerations;
using BlazorApp2.Services.Geocoding;
using BlazorApp2.BackgroundServices;
using BlazorApp2.Services.Jobs;

namespace BlazorApp2.Helpers;

public static class RegisterServices
{
    public static IServiceCollection AddMyServices(this IServiceCollection services)
    {
        services.AddScoped<ICrimeService, CrimeService>();
        services.AddScoped<IEnumeration, EnumerationService>();
        services.AddScoped<IClusteringService, ClusteringService>();
        services.AddScoped<IJobService, JobService>();
      

        return services;
    }

    public static IServiceCollection AddMyRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICrimeRepository, CrimeRepository>();
        services.AddScoped<ICrimeMotiveRepository, CrimeMotiveRepository>();
        services.AddScoped<ICrimeTypeRepository, CrimeTypeRepository>();
        services.AddScoped<IPoliceDistrictRepository, PoliceDistrictRepository>();
        services.AddScoped<ISeverityRepository, SeverityRepository>();
        services.AddScoped<IWeatherRepository, WeatherConRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();

        return services;
    }

    public static IServiceCollection AddMyBackgroundServices(this IServiceCollection services)
    {
        // Register the background service
        services.AddScoped<JobProcessingService>();
        services.AddScoped<AddressProcessorService>();
        //services.AddHostedService<UploadBackgroundService>();
        //services.AddHostedService<GISBackgroundService>();
        return services;
    }
}
