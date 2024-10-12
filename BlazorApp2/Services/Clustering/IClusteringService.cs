using BlazorApp2.Services.Crimes;

namespace BlazorApp2.Services.Clustering;

public interface IClusteringService
{
  
    List<ClusterResult> PerformKMeansClustering(IEnumerable<SanitizedCrimeRecord> data, string[] features, int numberOfClusters = 3);
    SanitizedCrimeRecord ToSanitizedCrimeRecord(CrimeDashboardDto crime);
}
