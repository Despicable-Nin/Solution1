using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories;

public class PoliceDistrictRepository(ApplicationDbContext dbContext) : IPoliceDistrictRepository
{
    public async Task AddPoliceDistrictsAsync(IEnumerable<PoliceDistrict> policeDistricts)
    {
        await dbContext.PoliceDistricts.AddRangeAsync(policeDistricts);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<PoliceDistrict>> GetPoliceDistrictsAsync() => await dbContext.PoliceDistricts.AsNoTracking().ToArrayAsync();
}
