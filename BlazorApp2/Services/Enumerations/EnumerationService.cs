using BlazorApp2.Components.Pages.Crimes;
using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Services.Enumerations
{
    public record CrimeTypeDto(string Title);
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

        public async Task<IDictionary<int,string>> AddCrimeTypes(string[] crimeTypesToAdd )
        {
            var crimeTypes = await GetCrimeTypes();
            var newCrimeTypes = crimeTypesToAdd.Except(crimeTypes.Select(c => c.Value)).ToArray();

            if (newCrimeTypes.Length != 0)
            {
                await AddRangeAsync(newCrimeTypes.Select(s => new CrimeType(s)));
            }
            return await GetCrimeTypes();
        }

        public async Task<IDictionary<int, string>> GetCrimeMotives(int? key = null)
        {
            IEnumerable<CrimeMotive> crimeMotives = [];
            if (key.HasValue)
            {
                crimeMotives = await crimeMotiveRepository.GetCrimeMotivesAsync();
                return crimeMotives.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
            }

            return crimeMotives.ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
        }

        public async Task<IDictionary<int, string>> GetCrimeTypes(int? key = null)
        {
            IEnumerable<CrimeType> crimeTypes = [];
            if (key.HasValue)
            {
                crimeTypes = await crimeTypeRepository.GetCrimeTypesAsync();
                return crimeTypes.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
            }

            return crimeTypes.ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
        }

        public async Task<IDictionary<int, string>> GetPoliceDistricts(int? key = null)
        {
            IEnumerable<PoliceDistrict> policeDistricts = [];
            if (key.HasValue)
            {
                policeDistricts = await policeDistrictRepository.GetPoliceDistrictsAsync();
                return policeDistricts.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
            }

            return policeDistricts.ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
        }

        public async Task<IDictionary<int, string>> GetSeverities(int? key = null)
        {
            IEnumerable<Severity> severities = [];
            if (key.HasValue)
            {
                severities = await severityRepository.GetSeveritiesAsync();
                return severities.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
            }

            return severities.ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
        }

        public async Task<IDictionary<int, string>> GetWeatherConditions(int? key = null)
        {
            IEnumerable<Weather> weatherConditions = [];
            if (key.HasValue)
            {
                weatherConditions = await weatherRepository.GetWeatherConditionsAsync ();
                return weatherConditions.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
            }

            return weatherConditions.ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
        }
    }
}
