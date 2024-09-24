using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories;

public class PoliceDistrictRepository(ApplicationDbContext dbContext) : IPoliceDistrictRepository
{
    public async Task<IEnumerable<PoliceDistrict>> GetPoliceDistrictsAsync() => await dbContext.PoliceDistricts.AsNoTracking().ToArrayAsync();
}
