using BlazorApp2.Data;

namespace BlazorApp2.Services.Clustering;

public interface IClusteringService
{
    Task AddFlatClusterAsync(SanitizedCrimeRecord flatCluster);
    Task AddFlatClusterRangeAsync(IEnumerable<SanitizedCrimeRecord> flatClusters);
    Task DeleteAllFlatClustersAsync();
    Task<IEnumerable<SanitizedCrimeRecord>> GetFlatClustersAsync();
    List<ClusterResult> PerformKMeansClustering(IEnumerable<SanitizedCrimeRecord> data, string[] features, int numberOfClusters = 3);
}
