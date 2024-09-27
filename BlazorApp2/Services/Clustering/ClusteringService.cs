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
        Log.Logger.Information("PerformKMeansClustering",data, features, numberOfClusters);
      
        // Convert the input data into an IDataView (ML.NET data structure)
        var schema = SchemaDefinition.Create(typeof(FlatCluster));

        schema["Id"].ColumnType = NumberDataViewType.Int32;

        var dataView = _mlContext.Data.LoadFromEnumerable(data, schema);

        var inputOutputColumnPairs = features.Select(x => new InputOutputColumnPair($"{x}_Single", x)).ToArray();
        Log.Logger.Information("inputOutputColumnPairs: {inputOutputColumnPairs}", inputOutputColumnPairs);

        var inputColumnNames = inputOutputColumnPairs.Select(x => x.OutputColumnName).ToArray();

       var pipeline = _mlContext.Transforms.Conversion.ConvertType(inputOutputColumnPairs, DataKind.Single) // Convert to Single (float)
        .Append(_mlContext.Transforms.Concatenate("Features", inputColumnNames))
        .Append(_mlContext.Clustering.Trainers.KMeans("Features", numberOfClusters: numberOfClusters));  // Specify number of clusters

        // Train the model
        var model = pipeline.Fit(dataView);

        // Make predictions
        var predictions = model.Transform(dataView);

        // Extract cluster assignments
        var clusterPredictions = _mlContext.Data.CreateEnumerable<ClusterPrediction>(predictions, reuseRowObject: false).ToList();

        return clusterPredictions.Select((p, index) => new ClusterResult
        {
            //RecordId = index + 1, // Can be based on your data's primary key
            ClusterId = p.PredictedClusterId,
            Latitude = p.Latitude,
            Longitude = p.Longitude,

        }).ToList();
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