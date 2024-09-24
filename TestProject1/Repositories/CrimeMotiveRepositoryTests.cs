using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp2.Data;
using BlazorApp2.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace TestProject1.Repositories
{
    public class CrimeMotiveRepositoryTests : BaseTest
    {
        private readonly CrimeMotiveRepository _repository;

        public CrimeMotiveRepositoryTests() : base()
        {
            _repository = new CrimeMotiveRepository(_dbContext);
        }

        [Fact]
        public async Task AddCrimeMotivesAsync_ShouldAddCrimeMotives()
        {
            // Arrange
            var crimeMotives = new List<CrimeMotive>
        {
            new CrimeMotive ("Theft"),
            new CrimeMotive("Assault")
        };

            // Act
            await _repository.AddCrimeMotivesAsync(crimeMotives);

            // Assert
            var addedMotives = await _dbContext.CrimeMotives.ToListAsync();
            Assert.Equal(2, addedMotives.Count);
            Assert.Contains(addedMotives, cm => cm.Title == "Theft");
            Assert.Contains(addedMotives, cm => cm.Title == "Assault");
        }

        [Fact]
        public async Task GetCrimeMotivesAsync_ShouldReturnAllCrimeMotives()
        {
            // Arrange
            _dbContext.CrimeMotives.AddRange(new List<CrimeMotive>
        {
             new CrimeMotive ("Theft"),
            new CrimeMotive("Assault")
        });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetCrimeMotivesAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, cm => cm.Title == "Theft");
            Assert.Contains(result, cm => cm.Title == "Assault");
        }
    }
}