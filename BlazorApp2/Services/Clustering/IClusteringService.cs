using BlazorApp2.Data;

namespace BlazorApp2.Services.Clustering;

public interface IClusteringService
{
    Task AddFlatClusterAsync(FlatCluster flatCluster);
    Task AddFlatClusterRangeAsync(IEnumerable<FlatCluster> flatClusters);
    Task DeleteAllFlatClustersAsync();
    Task<IEnumerable<FlatCluster>> GetFlatClustersAsync();
    List<ClusterResult> PerformKMeansClustering(IEnumerable<FlatCluster> data, string[] features, int numberOfClusters = 3);
}
