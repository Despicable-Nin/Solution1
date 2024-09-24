namespace BlazorApp2.Services.Enumerations
{
    public interface IEnumeration
    {
        IDictionary<int, string> GetCrimeMotivesAsync(int? key = null);
        IDictionary<int, string> GetCrimeTypesAsync(int? key = null);
        IDictionary<int, string> GetPoliceDistrictsAsync(int? key = null);
        IDictionary<int, string> GetSeveritiesAsync(int? key = null);
        IDictionary<int, string> GetWeatherConditionsAsync(int? key = null);

        Task AddRangeAsync<T>(params T[] values) where T : class;
    }
}
