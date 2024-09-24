using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories;

public class CrimeMotiveRepository(ApplicationDbContext context) : ICrimeMotiveRepository
{
    public async Task<IEnumerable<CrimeMotive>> GetCrimeMotivesAsync() => await context.CrimeMotives.AsNoTracking().ToArrayAsync();
}
