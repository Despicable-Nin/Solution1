
namespace BlazorApp2.Services.Geocoding
{
    public interface IAddressProcessorService
    {
        Task<(double? Latitude, double? Longitude)> GetLatLongAsync(string address);
    }
}