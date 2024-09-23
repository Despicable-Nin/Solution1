using BlazorApp2.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories
{
    public class CrimeMotiveRepository(ApplicationDbContext context)
    {
        public async Task<IEnumerable<CrimeMotive>> GetCrimeMotivesAsync()
        {
            return await context.CrimeMotives.AsNoTracking().ToArrayAsync();
        }
    }

}
