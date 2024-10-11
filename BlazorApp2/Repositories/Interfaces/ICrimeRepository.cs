using BlazorApp2.Components.Pages.Crimes;
using BlazorApp2.Data;

namespace BlazorApp2.Repositories.Interfaces;

public interface ICrimeRepository
{
    Task<(IEnumerable<Crime>, int totalCount)> GetCrimesAsync(int page, int pageSize);
    Task<IEnumerable<Crime>> GetCrimesByBatchIdAsync(Guid batchId);
    Task<Crime?> GetCrimeByCaseId(int id);

    Task<int> AddCrimesAsync(IEnumerable<Crime> crime);
    Task<Crime> UpdateCrimeAsync(Crime crime);
    Task DeleteCrimeAsync(Guid id);
    Task<IEnumerable<int>> GetExistingCaseIdsAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
