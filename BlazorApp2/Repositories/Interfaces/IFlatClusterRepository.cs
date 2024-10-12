using BlazorApp2.Services.Clustering;

namespace BlazorApp2.Repositories.Interfaces
{
    public interface IFlatClusterRepository
    {
        Task AddFlastClusterSingleAsync(SanitizedCrimeRecord flatCluster);
        Task AddFlatClustersAsync(IEnumerable<SanitizedCrimeRecord> flatClusters);
        Task DeleteAllFlatClustersAsync();
        Task<IEnumerable<SanitizedCrimeRecord>> GetFlatClustersAsync();

    }
}
