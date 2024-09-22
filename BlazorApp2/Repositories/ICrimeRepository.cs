using BlazorApp2.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories
{
    public interface ICrimeRepository
    {
        Task<(IEnumerable<Crime>, int totalCount)> GetCrimesAsync(int page, int pageSize);
        Task<IEnumerable<Crime>> GetCrimesAsync();
        Task<Crime> GetCrimeAsync(Guid id);
        Task<IEnumerable<string>> GetDistinctCrimeTypesAsync();
        Task AddCrimesAsync(IEnumerable<Crime> crime);
        Task<Crime> UpdateCrimeAsync(Crime crime);
        Task DeleteCrimeAsync(Guid id);
    }

    public class CrimeRepository(ApplicationDbContext dbContext) : ICrimeRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task AddCrimesAsync(IEnumerable<Crime> crime)
        {
            await _dbContext.Crimes.AddRangeAsync(crime);
            await _dbContext.SaveChangesAsync();
        }

        public Task DeleteCrimeAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Crime> GetCrimeAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<(IEnumerable<Crime>, int totalCount)> GetCrimesAsync(int page, int pageSize)
        {
            var totalCount = await _dbContext.Crimes.AsNoTracking().CountAsync();
            var paginatedCrimes = await _dbContext.Crimes.AsNoTracking().Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync();

            return (paginatedCrimes, totalCount);
        }

        public async Task<IEnumerable<Crime>> GetCrimesAsync()
        {
            return await _dbContext.Crimes.AsNoTracking().ToArrayAsync();
        }

        public async Task<IEnumerable<string>> GetDistinctCrimeTypesAsync() => await _dbContext.Crimes.AsNoTracking().Select(static c => c.CrimeType).Distinct().ToArrayAsync();

        public Task<Crime> UpdateCrimeAsync(Crime crime)
        {
            throw new NotImplementedException();
        }
    }
}
