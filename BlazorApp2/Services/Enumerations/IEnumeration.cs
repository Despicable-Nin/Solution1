using BlazorApp2.Data.Interfaces;

namespace BlazorApp2.Services.Enumerations
{
    public interface IEnumeration
    {
        Task<IDictionary<int, string>> GetCrimeMotives(int? key = null);
        Task<IDictionary<int, string>> GetCrimeTypes(int? key = null);
        Task<IDictionary<int, string>> GetPoliceDistricts(int? key = null);
        Task<IDictionary<int, string>> GetSeverities(int? key = null);
        Task<IDictionary<int, string>> GetWeatherConditions(int? key = null);

        Task<IDictionary<int, string>> AddCrimeTypes(IEnumerable<string> crimeTypesToAdd);
        Task<IDictionary<int, string>> AddCrimeMotives(IEnumerable<string> motivesToAdd);
        Task<IDictionary<int, string>> AddPoliceDistricts(IEnumerable<string> precinctsToAdd);
        Task<IDictionary<int, string>> AddWeatherConditions(IEnumerable<string> weatherConditionsToAdd);
        Task<IDictionary<int, string>> AddSeverities(IEnumerable<string> severitiesToAdd);
    }
}
