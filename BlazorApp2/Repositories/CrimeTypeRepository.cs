using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories;

public class CrimeTypeRepository (ApplicationDbContext dbContext) : ICrimeTypeRepository
{
    public async Task<IEnumerable<CrimeType>> GetCrimeTypesAsync() => await dbContext.CrimeTypes.AsNoTracking().ToArrayAsync();
}
