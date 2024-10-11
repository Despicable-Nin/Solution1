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

public class FlatClusterRepository(ApplicationDbContext context) : IFlatClusterRepository
{
    public async Task AddFlatClustersAsync(IEnumerable<SanitizedCrimeRecord> flatClusters)
    {
        context.SanitizedCrimeRecords.AddRange(flatClusters);
        await context.SaveChangesAsync();
    }

    public async Task AddFlastClusterSingleAsync(SanitizedCrimeRecord flatCluster)
    {
        context.SanitizedCrimeRecords.Add(flatCluster);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<SanitizedCrimeRecord>> GetFlatClustersAsync()
    {
        return await context.SanitizedCrimeRecords.AsNoTracking().ToArrayAsync();
    }

    public async Task DeleteAllFlatClustersAsync()
    {
        var allClusters = await context.SanitizedCrimeRecords.ToListAsync();
        context.SanitizedCrimeRecords.RemoveRange(allClusters);
        await context.SaveChangesAsync();
    }

}