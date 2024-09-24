using BlazorApp2.Data;

namespace BlazorApp2.Repositories.Interfaces;

public interface ICrimeRepository
{
    Task<(IEnumerable<Crime>, int totalCount)> GetCrimesAsync(int page, int pageSize);
    Task<IEnumerable<Crime>> GetCrimesAsync();
    Task<Crime> GetCrimeAsync(Guid id);

    Task AddCrimesAsync(IEnumerable<Crime> crime);
    Task<Crime> UpdateCrimeAsync(Crime crime);
    Task DeleteCrimeAsync(Guid id);
}
