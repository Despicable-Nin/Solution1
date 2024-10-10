using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories;

public class CrimeRepository(ApplicationDbContext dbContext) : ICrimeRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task AddCrimesAsync(IEnumerable<Crime> crime)
    {
        await _dbContext.Crimes.AddRangeAsync(crime);
        await _dbContext.SaveChangesAsync();
    }

    public Task DeleteCrimeAsync(Guid id) => throw new NotImplementedException();

    public async Task<Crime?> GetCrimeByCaseId(int id) => await _dbContext.Crimes.AsNoTracking().FirstOrDefaultAsync(c => c.CaseID == id);

    public async Task<IEnumerable<int>> GetExistingCaseIdsAsync()
    {
        return await _dbContext.Crimes.AsNoTracking().Select(c => c.CaseID).ToArrayAsync();
    }


    public async Task<(IEnumerable<Crime>, int totalCount)> GetCrimesAsync(int page, int pageSize)
    {
        var totalCount = _dbContext.Crimes.AsNoTracking().Count();
        var crimes = await  _dbContext.Crimes.AsNoTracking()
            .OrderBy(c => c.CaseID)
            .Skip(page -1).Take(pageSize)
            .ToArrayAsync();

        return (crimes, totalCount);
    }

    public Task<IEnumerable<Crime>> GetCrimesAsync() => throw new NotImplementedException();

    public Task<Crime> UpdateCrimeAsync(Crime crime) => throw new NotImplementedException();

    public Task<List<Crime>> GetExistingCaseIdsAsync(List<int> caseIds)
    {
        throw new NotImplementedException();
    }
}
