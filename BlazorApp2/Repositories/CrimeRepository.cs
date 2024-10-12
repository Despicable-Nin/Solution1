using BlazorApp2.Components.Pages.Crimes;
using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

namespace BlazorApp2.Repositories;

public class CrimeRepository(ApplicationDbContext dbContext) : ICrimeRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<int> AddCrimesAsync(IEnumerable<Crime> crime)
    {
        await _dbContext.Crimes.AddRangeAsync(crime);
        return await _dbContext.SaveChangesAsync();
    }

    public Task DeleteCrimeAsync(Guid id) => throw new NotImplementedException();

    public async Task<Crime?> GetCrimeByCaseId(int id) => await _dbContext.Crimes.AsNoTracking().FirstOrDefaultAsync(c => c.CaseID == id);

    public async Task<IEnumerable<int>> GetExistingCaseIdsAsync()
    {
        return await _dbContext.Crimes.AsNoTracking().Select(c => c.CaseID).ToArrayAsync();
    }


    public async Task<(IEnumerable<Crime>, int totalCount)> GetCrimesAsync(int page, int pageSize)
    {
        var totalCount = await _dbContext.Crimes.AsNoTracking().CountAsync();
        var crimes = await _dbContext.Crimes.AsNoTracking()
            .OrderBy(c => c.CaseID)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToArrayAsync();

        return (crimes, totalCount);
    }

    public async Task<IEnumerable<Crime>> GetCrimesByBatchIdAsync(Guid batchId)
    {
        var crimes = await dbContext.Crimes.AsNoTracking().Where(i => i.BatchId == batchId).ToArrayAsync();
        return crimes;
    }

    public async Task<Crime> UpdateCrimeAsync(Crime crime) {
        var entity = dbContext.Crimes.Update(crime).Entity;
        await dbContext.SaveChangesAsync(CancellationToken.None);

        return entity;
    }

    public Task<List<Crime>> GetExistingCaseIdsAsync(List<int> caseIds)
    {
        throw new NotImplementedException();
    }

    private static readonly object _lock = new object();
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                var result = dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(result);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
