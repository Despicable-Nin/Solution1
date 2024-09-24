using BlazorApp2.Data;

namespace BlazorApp2.Repositories.Interfaces;

public interface ICrimeMotiveRepository
{
    Task<IEnumerable<CrimeMotive>> GetCrimeMotivesAsync();
    Task AddCrimeMotivesAsync(IEnumerable<CrimeMotive> crimeMotives);
}
