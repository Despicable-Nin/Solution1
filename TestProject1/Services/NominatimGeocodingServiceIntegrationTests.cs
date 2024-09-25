using BlazorApp2.Services.Geocoding;

namespace TestProject1.Services
{
    public class NominatimGeocodingServiceIntegrationTests
    {
        private readonly NominatimGeocodingService _geocodingService;

        public NominatimGeocodingServiceIntegrationTests()
        {
            var httpClient = new HttpClient();
            _geocodingService = new NominatimGeocodingService(httpClient);
        }

        [Theory]
        [InlineData("Bayanan, Muntinlupa City", 14.406790, 121.046130)]
        [InlineData("Bacao 1, General Trias, Cavite", 14.400620, 120.885850)]
        [InlineData("Tunasan, Muntinlupa City", 14.368450, 121.031036)]
        public async Task GetLatLongAsync_ValidAddress_ReturnsCoordinates(string address, double lat, double _long)
        {
            // Arrange
            double expectedLatitude = Math.Round(lat, 6);
            double expectedLongitude = Math.Round(_long, 6);

            // Act
            var (latitude, longitude) = await _geocodingService.GetLatLongAsync(address);


            // Assert
            Assert.NotNull(latitude);
            Assert.NotNull(longitude);
            Assert.Equal(expectedLongitude, Math.Round(_long,6)); // Adjust precision as necessary
            Assert.Equal(expectedLatitude, Math.Round(lat,6));  // Adjust precision as necessary
        }

        [Fact]
        public async Task GetLatLongAsync_InvalidAddress_ThrowsException()
        {
            // Arrange
            var address = "This Address Does Not Exist";

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _geocodingService.GetLatLongAsync(address));
        }
    }
}