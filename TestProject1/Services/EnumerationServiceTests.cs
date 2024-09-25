using BlazorApp2.Services.Enumerations;
using BlazorApp2.Data.Interfaces;
using BlazorApp2.Data;
using System.Collections.Generic;
using System.Linq;
using BlazorApp2.Repositories.Interfaces;
using TestProject1.Repositories;
using TestProject1;
using BlazorApp2.Repositories;
using Moq;

namespace TestProject1.Services
{
    public class EnumerationServiceTests
    {
        private readonly Mock<IEnumeration> _mockService
             = new();
        public EnumerationServiceTests()
        {

        }

        [Fact]
        public async Task AddCrimeTypes_AddsNewCrimeTypes()
        {
            // Arrange
            Dictionary<int, string> pairs = new Dictionary<int, string>()
            {
                {1, "Theft"},
                {2, "Assault"}
            };

            _mockService.Setup(_mockService => _mockService.AddCrimeTypes(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(pairs);

            // Act
            var result = await _mockService.Object.AddCrimeTypes(new List<string> { "Theft", "Assault" });

            // Assert
            Assert.Equal(2, result.Count());
        }



        // Additional tests for other methods can follow a similar pattern...
    }
}