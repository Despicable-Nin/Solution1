using BlazorApp2.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Services.Enumerations
{
    public class EnumerationService : IEnumeration
    {
        private readonly ApplicationDbContext _dbContext;

        public EnumerationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task AddRangeAsync<T>(params T[] values) where T : class
        {
            ArgumentNullException.ThrowIfNull(values);

            _dbContext.Set<T>().AddRange(values);
            await _dbContext.SaveChangesAsync();
        }

        public IDictionary<int, string> GetCrimeMotivesAsync(int? key = null) => key == null ? 
            _dbContext.CrimeMotives.AsNoTracking().ToDictionary(x => x.Id, x => x.Title!) :
            _dbContext.CrimeMotives
            .AsNoTracking()
            .Where(x => x.Id == key.Value)
            .ToDictionary(x => x.Id, x => x.Title!) ;

        public IDictionary<int, string> GetCrimeTypesAsync(int? key = null) => key == null ?
            _dbContext.CrimeTypes.AsNoTracking().ToDictionary(x => x.Id, x => x.Title!) :
            _dbContext.CrimeTypes
            .AsNoTracking()
             .Where(x => x.Id == key.Value)
            .ToDictionary(x => x.Id, x => x.Title!);

        public IDictionary<int, string> GetPoliceDistrictsAsync(int? key = null) => key == null ?
            _dbContext.PoliceDistricts.AsNoTracking().ToDictionary(x => x.Id, x => x.Title!) :
            _dbContext.PoliceDistricts
            .AsNoTracking()
             .Where(x => x.Id == key.Value)
            .ToDictionary(x => x.Id, x => x.Title!);

        public IDictionary<int, string> GetSeveritiesAsync(int? key = null) => key == null ?
            _dbContext.Severity.AsNoTracking().ToDictionary(x => x.Id, x => x.Title!) :
            _dbContext.Severity
            .AsNoTracking()
             .Where(x => x.Id == key.Value)
            .ToDictionary(x => x.Id, x => x.Title!);

        public IDictionary<int, string> GetWeatherConditionsAsync(int? key = null) => key == null ?
            _dbContext.WeatherConditions.AsNoTracking().ToDictionary(x => x.Id, x => x.Title!) :
            _dbContext.WeatherConditions
            .AsNoTracking()
            .Where(x => x.Id == key.Value)
            .ToDictionary(x => x.Id, x => x.Title!);
    }
}
