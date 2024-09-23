using BlazorApp2.Data;

namespace BlazorApp2.Repositories
{
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

        public Task<(IEnumerable<Crime>, int totalCount)> GetCrimesAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Crime>> GetCrimesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Crime> UpdateCrimeAsync(Crime crime)
        {
            throw new NotImplementedException();
        }

    }
}
