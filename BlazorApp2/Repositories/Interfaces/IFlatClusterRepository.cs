using BlazorApp2.Data;

namespace BlazorApp2.Repositories.Interfaces
{
    public interface IFlatClusterRepository
    {
        Task AddFlastClusterSingleAsync(FlatCluster flatCluster);
        Task AddFlatClustersAsync(IEnumerable<FlatCluster> flatClusters);
        Task DeleteAllFlatClustersAsync();
        Task<IEnumerable<FlatCluster>> GetFlatClustersAsync();

    }
}
