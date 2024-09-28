using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.ML;
using Microsoft.ML.Data;
using Serilog;

namespace BlazorApp2.Services.Clustering;
public partial class ClusteringService(IFlatClusterRepository flatClusterRepository) : IClusteringService
{
    private readonly MLContext _mlContext = new();
    private readonly IFlatClusterRepository _flatClusterRepository = flatClusterRepository;

    public List<ClusterResult> PerformKMeansClustering(IEnumerable<FlatCluster> data, string[] features, int numberOfClusters = 3)
    {
        Log.Logger.Information("PerformKMeansClustering", data, features, numberOfClusters);

        // Convert the input data into an IDataView (ML.NET data structure)
        var schema = SchemaDefinition.Create(typeof(FlatCluster));

        schema["Id"].ColumnType = NumberDataViewType.Int32;

        var dataView = _mlContext.Data.LoadFromEnumerable(data, schema);

        var inputOutputColumnPairs = features.Select(x => new InputOutputColumnPair($"{x}_Single", x)).ToArray();
        Log.Logger.Information("inputOutputColumnPairs: {inputOutputColumnPairs}", inputOutputColumnPairs);

        var inputColumnNames = inputOutputColumnPairs.Select(x => x.OutputColumnName).ToArray();

        var pipeline = _mlContext.Transforms.Conversion.ConvertType(inputOutputColumnPairs, DataKind.Single) // Convert to Single (float)
         .Append(_mlContext.Transforms.Concatenate("Features", inputColumnNames))
         .Append(_mlContext.Clustering.Trainers.KMeans("Features", numberOfClusters: numberOfClusters ));  // Specify number of clusters

        // Train the model
        var model = pipeline.Fit(dataView);

        // Make predictions
        var predictions = model.Transform(dataView);

        var centroidModelParameters = model.LastTransformer.Model;

        // Get Centroids using `GetClusterCentroids`
        VBuffer<float>[] centroids = null;
        centroidModelParameters.GetClusterCentroids(ref centroids, out int numFeatures);

        // Output Centroids
        Console.WriteLine("Centroids:");
        for (int i = 0; i < centroids.Length; i++)
        {
            var centroidArray = centroids[i].DenseValues().ToArray();
            Console.WriteLine($"Cluster {i} Centroid: {string.Join(", ", centroidArray)}");
        }


        // Extract cluster assignments
        var clusterPredictions = _mlContext.Data.CreateEnumerable<ClusterPrediction>(predictions, reuseRowObject: false).ToList();

        Console.WriteLine("Cluster Assignments:");
        foreach (var cluster in clusterPredictions)
        {
            Console.WriteLine($"Cluster: {cluster.PredictedClusterId}, Features: {string.Join(", ", cluster.CaseId)}");
        }

        var temp = clusterPredictions
          .GroupBy(p => new {p.Latitude, p.Longitude}) // Group by cluster ID
          .Select(group => new ClusterResult// Create anonymous object for each group
          {
              Count = group.Count(), // Number of elements in the group
              Latitude = group.First().Latitude, // Latitude of the first element in the group
              Longitude = group.First().Longitude // Longitude of the first element in the group
          })
          .ToArray(); // Convert to an array

        return temp.OrderBy(x => x.Count).ToList();
    }

    //these methods are for the CRUD operations -- might move them to a separate service
    public async Task AddFlatClusterRangeAsync(IEnumerable<FlatCluster> flatClusters)
    {
        await _flatClusterRepository.AddFlatClustersAsync(flatClusters);
    }

    public async Task AddFlatClusterAsync(FlatCluster flatCluster)
    {
        await _flatClusterRepository.AddFlastClusterSingleAsync(flatCluster);
    }


    public async Task<IEnumerable<FlatCluster>> GetFlatClustersAsync()
    {
        return await _flatClusterRepository.GetFlatClustersAsync();
    }

    public async Task DeleteAllFlatClustersAsync()
    {
        await _flatClusterRepository.DeleteAllFlatClustersAsync();
    }
}