using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories;

public class CrimeMotiveRepository(ApplicationDbContext context) : ICrimeMotiveRepository
{
    public async Task AddCrimeMotivesAsync(IEnumerable<CrimeMotive> crimeMotives)
    {
        await context.CrimeMotives.AddRangeAsync(crimeMotives);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CrimeMotive>> GetCrimeMotivesAsync() => await context.CrimeMotives.AsNoTracking().ToArrayAsync();
}
