using BlazorApp2.Services.Enumerations;
using BlazorApp2.Data.Interfaces;
using BlazorApp2.Data;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using BlazorApp2.Repositories.Interfaces;

public class EnumerationServiceTests
{
    private readonly Mock<ICrimeMotiveRepository> _crimeMotiveRepoMock;
    private readonly Mock<ICrimeTypeRepository> _crimeTypeRepoMock;
    private readonly Mock<IPoliceDistrictRepository> _policeDistrictRepoMock;
    private readonly Mock<ISeverityRepository> _severityRepoMock;
    private readonly Mock<IWeatherRepository> _weatherRepoMock;
    private readonly EnumerationService _service;

    public EnumerationServiceTests()
    {
        _crimeMotiveRepoMock = new Mock<ICrimeMotiveRepository>();
        _crimeTypeRepoMock = new Mock<ICrimeTypeRepository>();
        _policeDistrictRepoMock = new Mock<IPoliceDistrictRepository>();
        _severityRepoMock = new Mock<ISeverityRepository>();
        _weatherRepoMock = new Mock<IWeatherRepository>();

        _service = new EnumerationService(
            _crimeMotiveRepoMock.Object,
            _crimeTypeRepoMock.Object,
            _policeDistrictRepoMock.Object,
            _severityRepoMock.Object,
            _weatherRepoMock.Object);
    }

    [Fact]
    public async Task AddCrimeTypes_AddsNewCrimeTypes()
    {
        // Arrange
        var existingCrimeTypes = new List<CrimeType>
        {
            new CrimeType("Robbery") { Id = 1 },
            new CrimeType("Assault") { Id = 2 }
        };
        _crimeTypeRepoMock.Setup(repo => repo.GetCrimeTypesAsync()).ReturnsAsync(existingCrimeTypes);

        var newCrimeTypes = new List<string> { "Burglary", "Fraud" };

        // Act
        var result = await _service.AddCrimeTypes(newCrimeTypes);

        // Assert
        _crimeTypeRepoMock.Verify(repo => repo.AddCrimeTypesAsync(It.IsAny<CrimeType[]>()), Times.Once);
        Assert.Equal(3, result.Count); // 2 existing + 2 new
    }

    [Fact]
    public async Task AddCrimeMotives_AddsNewCrimeMotives()
    {
        // Arrange
        var existingMotives = new List<CrimeMotive>
        {
            new CrimeMotive("Jealousy") { Id = 1 },
            new CrimeMotive("Greed") { Id = 2 }
        };
        _crimeMotiveRepoMock.Setup(repo => repo.GetCrimeMotivesAsync()).ReturnsAsync(existingMotives);

        var newMotives = new List<string> { "Revenge", "Desperation" };

        // Act
        var result = await _service.AddCrimeMotives(newMotives);

        // Assert
        _crimeMotiveRepoMock.Verify(repo => repo.AddCrimeMotivesAsync(It.IsAny<CrimeMotive[]>()), Times.Once);
        Assert.Equal(4, result.Count); // 2 existing + 2 new
    }

    // Additional tests for other methods can follow a similar pattern...
}
