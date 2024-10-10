using BlazorApp2.Components.Pages.Crimes;
using BlazorApp2.Data;
using BlazorApp2.Repositories;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1.Repositories
{
    public class CrimeRepositoryTests : BaseTest
    {
        private readonly ICrimeRepository _crimeRepository;

        public CrimeRepositoryTests() : base()
        {
            _crimeRepository = new CrimeRepository(_dbContext);
        }

        [Fact]
        public async Task AddCrimesAsync_AddsCrimesToDatabase()
        {
            // Arrange
            var crimes = new List<Crime>
            {
                new Crime { Id = Guid.NewGuid(), Description = "Theft", Date = DateTime.Now },
                new Crime { Id = Guid.NewGuid(), Description = "Assault", Date = DateTime.Now }
            };

            // Act
            await _crimeRepository.AddCrimesAsync(crimes);

            // Assert
            var dbCrimes = await _dbContext.Crimes.ToListAsync();
            Assert.Equal(2, dbCrimes.Count); // Check if 2 crimes are added
            Assert.Contains(dbCrimes, c => c.Description == "Theft");
            Assert.Contains(dbCrimes, c => c.Description == "Assault");
        }

        [Fact]
        public async Task GetCrimesAsync_ReturnsPagedCrimes()
        {
            // Arrange
            _dbContext.Crimes.AddRange(new List<Crime>
            {
                new Crime { Id = Guid.NewGuid(), Description = "Crime 1", Date = DateTime.Now },
                new Crime { Id = Guid.NewGuid(), Description = "Crime 2", Date = DateTime.Now },
                new Crime { Id = Guid.NewGuid(), Description = "Crime 3", Date = DateTime.Now },
                new Crime { Id = Guid.NewGuid(), Description = "Crime 4", Date = DateTime.Now }
            });
            await _dbContext.SaveChangesAsync();

            int page = 0;
            int pageSize = 2;

            // Act
            var (crimes, totalCount) = await _crimeRepository.GetCrimesAsync(page, pageSize);

            // Assert
            Assert.Equal(4, totalCount); // Check if the total count is correct
            Assert.Equal(2, crimes.Count()); // Ensure the correct number of crimes are returned
        }

        [Fact]
        public async Task GetCrimesAsync_ReturnsEmptyWhenPageExceedsCount()
        {
            // Arrange
            _dbContext.Crimes.AddRange(new List<Crime>
            {
                new Crime { Id = Guid.NewGuid(), Description = "Crime 1", Date = DateTime.Now },
                new Crime { Id = Guid.NewGuid(), Description = "Crime 2", Date = DateTime.Now }
            });
            await _dbContext.SaveChangesAsync();

            int page = 1; // Second page
            int pageSize = 5; // Larger than the total count

            // Act
            var (crimes, totalCount) = await _crimeRepository.GetCrimesAsync(page, pageSize);

            // Assert
            Assert.Equal(2, totalCount); // Total count should still be correct
            Assert.Empty(crimes); // No crimes should be returned because the page exceeds available crimes
        }
    }
}
