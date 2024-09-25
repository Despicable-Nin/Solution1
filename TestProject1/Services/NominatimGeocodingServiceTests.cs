namespace TestProject1.Services
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BlazorApp2.Services.Geocoding;
    using Moq;
    using Moq.Protected;
    using Xunit;

    public class NominatimGeocodingServiceTests
    {
        [Fact]
        public async Task GetLatLongAsync_ValidAddress_ReturnsCoordinates()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{\"lat\":\"37.4224764\",\"lon\":\"-122.0842499\",\"display_name\":\"1600 Amphitheatre Parkway, Mountain View, CA, USA\"}]"),
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var geocodingService = new NominatimGeocodingService(httpClient);

            // Act
            var (latitude, longitude) = await geocodingService.GetLatLongAsync("1600 Amphitheatre Parkway, Mountain View, CA");

            // Assert
            Assert.Equal(37.4224764, latitude);
            Assert.Equal(-122.0842499, longitude);

            // Verify that the SendAsync method was called exactly once
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetLatLongAsync_InvalidAddress_ThrowsException()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[]"), // No results returned
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var geocodingService = new NominatimGeocodingService(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => geocodingService.GetLatLongAsync("Invalid Address"));

            // Verify that the SendAsync method was called exactly once
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
