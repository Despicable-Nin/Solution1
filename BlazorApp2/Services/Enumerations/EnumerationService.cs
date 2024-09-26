using BlazorApp2.Components.Pages.Crimes;
using BlazorApp2.Data;
using BlazorApp2.Data.Interfaces;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Services.Enumerations
{
    public record CrimeTypeDto(string Title);
    public class EnumerationService(ICrimeMotiveRepository crimeMotiveRepository, ICrimeTypeRepository crimeTypeRepository, IPoliceDistrictRepository policeDistrictRepository, ISeverityRepository severityRepository, IWeatherRepository weatherRepository) : IEnumeration
    {
        public async Task AddRangeAsync(params IEntity[] values)
        {
            ArgumentNullException.ThrowIfNull(values);

            if (values.All(v => v is CrimeMotive))
                await crimeMotiveRepository.AddCrimeMotivesAsync(values as CrimeMotive[]);
            else if (values.All(v => v is CrimeType))
                await crimeTypeRepository.AddCrimeTypesAsync(values.Cast<CrimeType>());
            else if (values.All(v => v is PoliceDistrict))
                await policeDistrictRepository.AddPoliceDistrictsAsync(values.Cast<PoliceDistrict>());
            else if (values.All(v => v is Severity))
                await severityRepository.AddSeveritiesAsync(values.Cast<Severity>());
            else if (values.All(v => v is Weather))
                await weatherRepository.AddWeatherConditionsAsync(values.Cast<Weather>());
            else
                throw new ArgumentException("Invalid type");
        }

        /// <summary>
        /// Add crime types to the database
        /// </summary>
        /// <param name="crimeTypesToAdd"></param>
        /// <returns></returns>
        public async Task<IDictionary<int,string>> AddCrimeTypes(IEnumerable<string> crimeTypesToAdd )
        {
            try
            {
                var crimeTypes = await GetCrimeTypes();
                var newCrimeTypes = crimeTypesToAdd.Except(crimeTypes.Select(c => c.Value)).ToArray();

                if (newCrimeTypes.Length != 0)
                {
                    await AddRangeAsync(newCrimeTypes.Select(s => new CrimeType(s)).ToArray());
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return await GetCrimeTypes();
        }

        /// <summary>
        /// Add crime motives to the database
        /// </summary>
        /// <param name="motivesToAdd"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> AddCrimeMotives(IEnumerable<string> motivesToAdd)
        {
            try
            {
                var motives = await GetCrimeMotives();
                var newMotives = motivesToAdd.Except(motives.Select(c => c.Value)).ToArray();

                if (newMotives.Length != 0)
                {
                    await AddRangeAsync(newMotives.Select(s => new CrimeMotive(s)).ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return await GetCrimeMotives();
        }

        /// <summary>
        /// Add police districts to the database
        /// </summary>
        /// <param name="precinctsToAdd"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> AddPoliceDistricts(IEnumerable<string> precinctsToAdd)
        {
            try
            {
                var precincts = await GetPoliceDistricts();
                var newPrecincts = precinctsToAdd.Except(precincts.Select(c => c.Value)).ToArray();

                if (newPrecincts.Length != 0)
                {
                    await AddRangeAsync(newPrecincts.Select(s => new PoliceDistrict(s)).ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return await GetPoliceDistricts();
        }

        /// <summary>
        /// Add severities to the database
        /// </summary>
        /// <param name="severitiesToAdd"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> AddSeverities(IEnumerable<string> severitiesToAdd)
        {
            try
            {
                var severities = await GetSeverities();
                var newSeverities = severitiesToAdd.Except(severities.Select(c => c.Value)).ToArray();

                if (newSeverities.Length != 0)
                {
                    await AddRangeAsync(newSeverities.Select(s => new Severity(s)).ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return await GetSeverities();
        }

        /// <summary>
        /// Add weather conditions to the database
        /// </summary>
        /// <param name="weatherConditionsToAdd"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> AddWeatherConditions(IEnumerable<string> weatherConditionsToAdd)
        {
            try
            {
                var weatherConditions = await GetWeatherConditions();
                var newWeatherConditions = weatherConditionsToAdd.Except(weatherConditions.Select(c => c.Value)).ToArray();

                if (newWeatherConditions.Length != 0)
                {
                    await AddRangeAsync(newWeatherConditions.Select(s => new Weather(s)).ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return await GetWeatherConditions();
        }

        /// <summary>
        /// // Get crime motives from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> GetCrimeMotives(int? key = null)
        {
           IEnumerable<CrimeMotive> crimeMotives = await crimeMotiveRepository.GetCrimeMotivesAsync();
           return key.HasValue ? 
                crimeMotives.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty) : 
                crimeMotives.ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
        }

        /// <summary>
        /// // Get crime types from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> GetCrimeTypes(int? key = null)
        {
            IEnumerable<CrimeType> crimeTypes = await crimeTypeRepository.GetCrimeTypesAsync();
          return key.HasValue ?
                crimeTypes.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty) :
                crimeTypes.ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
           
        }

        /// <summary>
        /// // Get police districts from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> GetPoliceDistricts(int? key = null)
        {
            IEnumerable<PoliceDistrict> policeDistricts = [];
            return key.HasValue ? 
                policeDistricts.ToDictionary(x => x.Id, x => x.Title ?? string.Empty) :
                policeDistricts.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
        }

        /// <summary>
        /// // Get severities from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> GetSeverities(int? key = null)
        {
            IEnumerable<Severity> severities = [];
            return key.HasValue ? 
                severities.ToDictionary(x => x.Id, x => x.Title ?? string.Empty) :
                severities.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
      
        }

        /// <summary>
        /// // Get weather conditions from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> GetWeatherConditions(int? key = null)
        {
            IEnumerable<Weather> weatherConditions = [];
            return key.HasValue ? 
                weatherConditions.ToDictionary(x => x.Id, x => x.Title ?? string.Empty) :
                weatherConditions.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
        }
    }
}
