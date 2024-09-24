namespace BlazorApp2.Services.Enumerations
{
    public interface IEnumeration
    {
        IDictionary<int, string> GetCrimeMotives(int? key = null);
        IDictionary<int, string> GetCrimeTypes(int? key = null);
        IDictionary<int, string> GetPoliceDistricts(int? key = null);
        IDictionary<int, string> GetSeverities(int? key = null);
        IDictionary<int, string> GetWeatherConditions(int? key = null);

        Task AddRangeAsync<T>(params T[] values) where T : class;
    }
}
