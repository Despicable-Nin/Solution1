using BlazorApp2.Repositories.Interfaces;
using Serilog;
using System.Text.RegularExpressions;

namespace BlazorApp2.Services.Geocoding
{
    public class AddressProcessorService 
    {
        private readonly HttpClient _httpClient;
        private readonly IAddressRepository _addressRepository;

        // Regex to target typical house numbers (e.g., "123", "123A", "123-B")
        private static readonly Regex HouseNumberRegex = new Regex(@"^\d+\s*\w*", RegexOptions.Compiled);
        private static readonly Regex StreetAbbreviationRegex = new Regex(@"\bSt\.?\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex IllegalCharacterRegex = new Regex(@"[^\w\s,.-]", RegexOptions.Compiled);
        private static readonly Regex MultiSpaceRegex = new Regex(@"\s+", RegexOptions.Compiled);
        private static readonly Regex PostalCodeRegex = new Regex(@"\d{4,5}(\s)?$", RegexOptions.Compiled);
        private static readonly Regex CountryNameRegex = new Regex(@"\b(PH|PHILIPPINES|PHIL)\b$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex BarangayRegex = new Regex(@"\b(Barangay|baranggay|brgy|brgy\.)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public AddressProcessorService(HttpClient httpClient, IAddressRepository addressRepository)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "YourAppName/1.0");
            _addressRepository = addressRepository;
        }

        public async Task<(string SanitizedAddress, (double? Latitude, double? Longitude))> GetLatLongAsync(string address)
        {
            var sanitizedAddress = SanitizeAddress(address.Replace(";", ","));

            for (int attempt = 1; attempt <= 5; attempt++)
            {
                var response = await MakeNominatimRequestPH(sanitizedAddress);

                if (response != null && response.Length > 0)
                {
                    var location = response[0];
                    var lat = double.Parse(location.Lat);
                    var lon = double.Parse(location.Lon);
                    return (sanitizedAddress,(lat, lon));
                }

                sanitizedAddress = TrimFurther(sanitizedAddress);

                if (attempt < 5)
                {
                    await Task.Delay(1000); // Delay for 1 second
                }
            }

            return (null, (0,0));
        }

        private async Task<NominatimResult[]?> MakeNominatimRequestPH(string address)
        {
            var requestUrl = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}, Philippines";
            try
            {
                var response = await _httpClient.GetFromJsonAsync<NominatimResult[]>(requestUrl);
                return response;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Exception occurred while making Nominatim request");
            }

            return null;
        }

        private static string SanitizeAddress(string address)
        {
            address = HouseNumberRegex.Replace(address, string.Empty).Trim();
            address = StreetAbbreviationRegex.Replace(address, "Street");
            address = IllegalCharacterRegex.Replace(address, " ");
            address = MultiSpaceRegex.Replace(address, " ");
            address = PostalCodeRegex.Replace(address, "").Trim();
            address = CountryNameRegex.Replace(address, "", 1).Trim();
            address = BarangayRegex.Replace(address, "").Trim();
            if (address.EndsWith(','))
            {
                address = address.Substring(0, address.Length - 1).Replace(".", "");
            }
            return string.Join(", ", address.Split(',').Select(part => part.Trim())).ToUpper();
        }

        private static string TrimFurther(string address)
        {
            var parts = address.Split(',');
            if (parts.Length > 1)
            {
                address = string.Join(",", parts[1..]).Trim();
            }
            return address.ToUpper();
        }
    }

    public record NominatimResult(string Lat, string Lon, string DisplayName);
}
