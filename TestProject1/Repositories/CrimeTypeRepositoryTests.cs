using BlazorApp2.Data;
using BlazorApp2.Repositories;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TestProject1.Repositories
{
    public class CrimeTypeRepositoryTests : BaseTest
    {
        private readonly ICrimeTypeRepository _crimeTypeRepository;

        public CrimeTypeRepositoryTests() : base()
        {
            _crimeTypeRepository = new CrimeTypeRepository(_dbContext); ;
        }

        [Fact]
        public async Task AddCrimeTypesAsync_AddsCrimeTypesToDatabase()
        {
            // Arrange
            var crimeTypes = new List<CrimeType>
            {
                new CrimeType ("Theft"),
                new CrimeType ("Assault")
            };

            // Act
            await _crimeTypeRepository.AddCrimeTypesAsync(crimeTypes);

            // Assert
            var dbCrimeTypes = await _dbContext.CrimeTypes.ToListAsync();
            Assert.Equal(2, dbCrimeTypes.Count); // Check if 2 crime types are added
            Assert.Contains(dbCrimeTypes, ct => ct.Title == "Theft");
            Assert.Contains(dbCrimeTypes, ct => ct.Title == "Assault");
            Assert.True(dbCrimeTypes.First(x => x.Id == 1).Title == "Theft"); // Check if Id is set (not empty
        }

        [Fact]
        public async Task GetCrimeTypesAsync_ReturnsAllCrimeTypes()
        {
            // Arrange
            _dbContext.CrimeTypes.AddRange(new List<CrimeType>
            {
                new CrimeType ("Crime Type 1"),
                new CrimeType ("Crime Type 2")
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var crimeTypes = await _crimeTypeRepository.GetCrimeTypesAsync();

            // Assert
            Assert.Equal(2, crimeTypes.Count()); // Check if all crime types are returned
            Assert.Contains(crimeTypes, ct => ct.Title == "Crime Type 1");
            Assert.Contains(crimeTypes, ct => ct.Title == "Crime Type 2");
        }
    }
}
