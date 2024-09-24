using BlazorApp2.Repositories.Interfaces;
using BlazorApp2.Repositories;
using BlazorApp2.Services.Clustering;
using BlazorApp2.Services.Crimes;
using BlazorApp2.Services.Enumerations;

namespace BlazorApp2.Helpers;

public static class IServicesCollectionExtensions
{
    public static IServiceCollection AddMyServices(this IServiceCollection services)
    {
        services.AddScoped<ICrimeService, CrimeService>();
        services.AddScoped<IEnumeration, EnumerationService>();
        services.AddSingleton<ClusteringService>();

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
        services.AddSingleton<ClusteringService>();

        return services;
    }
}
