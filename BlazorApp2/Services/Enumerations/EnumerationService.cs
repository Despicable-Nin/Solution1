using BlazorApp2.Components.Pages.Crimes;
using BlazorApp2.Data;
using BlazorApp2.Data.Interfaces;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BlazorApp2.Services.Enumerations
{
    public record CrimeTypeDto(string Title);
    public class EnumerationService(ICrimeMotiveRepository crimeMotiveRepository, ICrimeTypeRepository crimeTypeRepository, IPoliceDistrictRepository policeDistrictRepository, ISeverityRepository severityRepository, IWeatherRepository weatherRepository) : IEnumeration
    {
        /// <summary>
        /// Add crime types to the database
        /// </summary>
        /// <param name="crimeTypesToAdd"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> AddCrimeTypes(IEnumerable<string> crimeTypesToAdd)
        {
            try
            {
                var crimeTypes = await GetCrimeTypes();
                var existingCrimeTypes = crimeTypes.Select(i => i.Value.ToLower()).ToHashSet();
                var newCrimeTypes = crimeTypesToAdd.Where(nuCrime => !existingCrimeTypes.Contains(nuCrime.ToLower())).ToArray();

                if (newCrimeTypes.Length != 0)
                {
                    await crimeTypeRepository.AddCrimeTypesAsync(newCrimeTypes.Select(s => new CrimeType(s)).ToArray());
                }

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message, ex.InnerException.Message);
            }
            return await GetCrimeTypes();
        }

        /// <summary>
        /// Add crime precincts to the database
        /// </summary>
        /// <param name="motivesToAdd"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> AddCrimeMotives(IEnumerable<string> motivesToAdd)
        {
            try
            {
                var motives = await GetCrimeMotives();
                var existingMotiveTypes = motives.Select(i => i.Value.ToLower()).ToHashSet();
                var newMotives = motivesToAdd.Where(newMotives => !existingMotiveTypes.Contains(newMotives.ToLower())).ToArray();

                if (newMotives.Length != 0)
                {
                    await crimeMotiveRepository.AddCrimeMotivesAsync(newMotives.Select(s => new CrimeMotive(s)).ToArray());
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message, ex.InnerException.Message);
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
                var existingPrecincts = precincts.Select(i => i.Value.ToLower()).ToHashSet();
                var newMotives = precinctsToAdd.Where(newMotives => !existingPrecincts.Contains(newMotives.ToLower())).ToArray();

                if (newMotives.Length != 0)
                {
                    await policeDistrictRepository.AddPoliceDistrictsAsync(newMotives.Select(s => new PoliceDistrict(s)).ToArray());
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message, ex.InnerException.Message);
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
                // Fetch existing severities
                var severities = await GetSeverities();
                var existingSeverities = severities.Select(i => i.Value.ToLower()).ToHashSet();

                // Filter new severities to add only those not in existing severities
                var newSeverities = severitiesToAdd
                    .Where(nuSeverity => !existingSeverities.Contains(nuSeverity.ToLower()))
                    .ToArray();

                // Add new severities if there are any
                if (newSeverities.Length != 0)
                {
                    await severityRepository.AddSeveritiesAsync(newSeverities.Select(s => new Severity(s)).ToArray());
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Logger.Error(ex.Message, ex.InnerException.Message);
            }

            // Return the updated list of severities
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
                // Fetch existing weather conditions
                var weatherConditions = await GetWeatherConditions();
                var existingWeatherConditions = weatherConditions.Select(i => i.Value.ToLower()).ToHashSet();

                // Filter new weather conditions to add only those not in existing weather conditions
                var newWeatherConditions = weatherConditionsToAdd
                    .Where(nuCondition => !existingWeatherConditions.Contains(nuCondition.ToLower()))
                    .ToArray();

                // Add new weather conditions if there are any
                if (newWeatherConditions.Length != 0)
                {
                    await weatherRepository.AddWeatherConditionsAsync(newWeatherConditions.Select(s => new Weather(s)).ToArray());
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
            }

            // Return the updated list of weather conditions
            return await GetWeatherConditions();
        }


        /// <summary>
        /// // Get crime precincts from the database
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
            IEnumerable<PoliceDistrict> policeDistricts = await policeDistrictRepository.GetPoliceDistrictsAsync();
            return key.HasValue ?
                policeDistricts.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty) :
                policeDistricts.ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
        }

        /// <summary>
        /// // Get severities from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> GetSeverities(int? key = null)
        {
            IEnumerable<Severity> severities = await severityRepository.GetSeveritiesAsync();
            return key.HasValue ?
                severities.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty) :
                severities.ToDictionary(x => x.Id, x => x.Title ?? string.Empty);

        }

        /// <summary>
        /// // Get weather conditions from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, string>> GetWeatherConditions(int? key = null)
        {
            IEnumerable<Weather> weatherConditions = await weatherRepository.GetWeatherConditionsAsync();
            return key.HasValue ?
                weatherConditions.Where(x => x.Id == key).ToDictionary(x => x.Id, x => x.Title ?? string.Empty) :
                weatherConditions.ToDictionary(x => x.Id, x => x.Title ?? string.Empty);
        }
    }
}
