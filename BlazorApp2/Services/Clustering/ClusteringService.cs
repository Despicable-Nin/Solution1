using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using BlazorApp2.Services.Crimes;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
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

        //we are making this non-negotiable
        //var pipeline = _mlContext.Transforms.Conversion.ConvertType(outputColumnName: "CaseID_Single", inputColumnName: "CaseID", outputKind: DataKind.Single);

        var inputOutputColumnPairs = features.Select(x => new InputOutputColumnPair($"{x}_Single", x)).ToArray();
        Log.Logger.Information("inputOutputColumnPairs: {inputOutputColumnPairs}", inputOutputColumnPairs);

        var inputColumnNames = inputOutputColumnPairs.Select(x => x.OutputColumnName).ToArray();

        IEstimator<ITransformer> pipeline = _mlContext.Transforms.Conversion.ConvertType(inputOutputColumnPairs, DataKind.Single) // Convert to Single (float)
        .Append(_mlContext.Transforms.Concatenate("Features", inputColumnNames))
        .Append(_mlContext.Clustering.Trainers.KMeans("Features", numberOfClusters: 3));  // Specify number of clusters

        //Append(features, pipeline);

        // Train the model
        var model = pipeline.Fit(dataView);

        // Make predictions
        var predictions = model.Transform(dataView);

        // Extract cluster assignments
        var clusterPredictions = _mlContext.Data.CreateEnumerable<ClusterPrediction>(predictions, reuseRowObject: false).ToList();

        return clusterPredictions.Select((p, index) => new ClusterResult
        {
            RecordId = index + 1, // Can be based on your data's primary key
            ClusterId = p.PredictedClusterId,


        }).ToList();
    }

    private void Append(string[] features, IEstimator<ITransformer> pipeline)
    {
        var hotEncodes = new List<string>();
        var transformers = new List<string>();
        foreach (var feature in features)
        {
            string[] categoricals = [
                nameof(FlatCluster.CrimeMotive),
                nameof(FlatCluster.CrimeType),
                nameof(FlatCluster.Severity),
                nameof(FlatCluster.WeatherCondition),
                nameof(FlatCluster.PoliceDistrict)
                ];

            if (categoricals.Contains(feature))
            {
                hotEncodes.Add(feature);
            }

            transformers.Add($"{feature}_Single");
            pipeline.Append(_mlContext.Transforms.Conversion.ConvertType(outputColumnName: $"{feature}_Single", inputColumnName: feature, outputKind: DataKind.Single));
        }
        pipeline.Append(_mlContext.Transforms.Concatenate("Features", transformers.ToArray()));

        foreach (var hotties in hotEncodes)
        {
            pipeline.Append(_mlContext.Transforms.Categorical.OneHotEncoding(hotties));
        }
        pipeline.Append(_mlContext.Clustering.Trainers.KMeans(
                  featureColumnName: "Features",
                  numberOfClusters: 3));
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