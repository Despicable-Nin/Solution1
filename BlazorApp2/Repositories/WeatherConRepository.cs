using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories;

public class WeatherConRepository(ApplicationDbContext context) : IWeatherRepository
{
    public async Task<IEnumerable<Weather>> GetWeatherConditionsAsync() => await context.WeatherConditions.AsNoTracking().ToArrayAsync();

  
}
