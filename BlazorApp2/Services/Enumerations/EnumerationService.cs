using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Services.Enumerations
{
    public class EnumerationService(ICrimeMotiveRepository crimeMotiveRepository, ICrimeTypeRepository crimeTypeRepository, IPoliceDistrictRepository policeDistrictRepository, ISeverityRepository severityRepository, IWeatherRepository weatherRepository) : IEnumeration
    {
        public async Task AddRangeAsync<T>(params T[] values) where T : class
        {
            ArgumentNullException.ThrowIfNull(values);

            if (typeof(T) == typeof(CrimeMotive))
                await crimeMotiveRepository.AddCrimeMotivesAsync(values.Cast<CrimeMotive>());
            else if (typeof(T) == typeof(CrimeType))
                await crimeTypeRepository.AddCrimeTypesAsync(values.Cast<CrimeType>());
            else if (typeof(T) == typeof(PoliceDistrict))
                await policeDistrictRepository.AddPoliceDistrictsAsync(values.Cast<PoliceDistrict>());
            else if (typeof(T) == typeof(Severity))
                await severityRepository.AddSeveritiesAsync(values.Cast<Severity>());
            else if (typeof(T) == typeof(Weather))
                await weatherRepository.AddWeatherConditionsAsync(values.Cast<Weather>());
            else
                throw new ArgumentException($"Type {typeof(T).Name} is not supported", nameof(T));
        }

        public IDictionary<int, string> GetCrimeMotives(int? key = null) => key == null ? 
            _dbContext.CrimeMotives.AsNoTracking().ToDictionary(x => x.Id, x => x.Title!) :
            _dbContext.CrimeMotives
            .AsNoTracking()
            .Where(x => x.Id == key.Value)
            .ToDictionary(x => x.Id, x => x.Title!) ;

        public IDictionary<int, string> GetCrimeTypes(int? key = null) => key == null ?
            _dbContext.CrimeTypes.AsNoTracking().ToDictionary(x => x.Id, x => x.Title!) :
            _dbContext.CrimeTypes
            .AsNoTracking()
             .Where(x => x.Id == key.Value)
            .ToDictionary(x => x.Id, x => x.Title!);

        public IDictionary<int, string> GetPoliceDistricts(int? key = null) => key == null ?
            _dbContext.PoliceDistricts.AsNoTracking().ToDictionary(x => x.Id, x => x.Title!) :
            _dbContext.PoliceDistricts
            .AsNoTracking()
             .Where(x => x.Id == key.Value)
            .ToDictionary(x => x.Id, x => x.Title!);

        public IDictionary<int, string> GetSeverities(int? key = null) => key == null ?
            _dbContext.Severity.AsNoTracking().ToDictionary(x => x.Id, x => x.Title!) :
            _dbContext.Severity
            .AsNoTracking()
             .Where(x => x.Id == key.Value)
            .ToDictionary(x => x.Id, x => x.Title!);

        public IDictionary<int, string> GetWeatherConditions(int? key = null) => key == null ?
            _dbContext.WeatherConditions.AsNoTracking().ToDictionary(x => x.Id, x => x.Title!) :
            _dbContext.WeatherConditions
            .AsNoTracking()
            .Where(x => x.Id == key.Value)
            .ToDictionary(x => x.Id, x => x.Title!);
    }
}
