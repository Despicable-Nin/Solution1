using BlazorApp2.Data;

namespace BlazorApp2.Repositories.Interfaces;

public interface IWeatherRepository
{
    Task<IEnumerable<Weather>> GetWeatherConditionsAsync();
    Task AddWeatherConditionsAsync(IEnumerable<Weather> weatherConditions);
}
