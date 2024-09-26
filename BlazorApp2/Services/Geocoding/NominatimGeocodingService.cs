

namespace BlazorApp2.Services.Geocoding
{
    public class NominatimGeocodingService
    {
        private readonly HttpClient _httpClient;

        public NominatimGeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Set the User-Agent header
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "YourAppName/1.0");
        }

        public async Task<(double Latitude, double Longitude)> GetLatLongAsync(string address)
        {
            var requestUrl = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}";

            var response = await _httpClient.GetFromJsonAsync<NominatimResult[]>(requestUrl);

            if (response != null && response.Length > 0)
            {
                var location = response[0]; // Get the first result
                return (double.Parse(location.Lat), double.Parse(location.Lon));
            }

            throw new Exception("Unable to get coordinates for the given address.");
        }
    }

    public record NominatimResult(string Lat, string Lon, string DisplayName);

}