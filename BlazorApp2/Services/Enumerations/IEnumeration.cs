namespace BlazorApp2.Services.Enumerations
{
    public interface IEnumeration
    {
        Task<IDictionary<int, string>> GetCrimeMotives(int? key = null);
        Task<IDictionary<int, string>> GetCrimeTypes(int? key = null);
        Task<IDictionary<int, string>> GetPoliceDistricts(int? key = null);
        Task<IDictionary<int, string>> GetSeverities(int? key = null);
        Task<IDictionary<int, string>> GetWeatherConditions(int? key = null);

        Task AddRangeAsync<T>(params T[] values) where T : class;
    }
}
