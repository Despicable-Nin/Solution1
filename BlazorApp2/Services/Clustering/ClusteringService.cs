using BlazorApp2.Services.Crimes;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace BlazorApp2.Services.Clustering;

public class ClusteringService
{
    private readonly MLContext _mlContext;

    public ClusteringService()
    {
        _mlContext = new MLContext();
    }

    public List<ClusterResult> PerformKMeansClustering(IEnumerable<CrimeDto> data)
    {
        // Convert the input data into an IDataView (ML.NET data structure)
        var schema = SchemaDefinition.Create(typeof(CrimeDto));

        // Define ArrestDate as a string field
        schema["ArrestDate"].ColumnType = TextDataViewType.Instance;

        var dataView = _mlContext.Data.LoadFromEnumerable(data, schema);


        // Define the pipeline: concatenate features and apply K-Means clustering
        // Features should be integer or float values
        //var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(CrimeDto.VictimCount), nameof(CrimeDto.ArrestMade), nameof(CrimeDto.RecurringIncident))//, nameof(CrimeData.LocationY))
        //    .Append(_mlContext.Clustering.Trainers.KMeans("Features", numberOfClusters: 3));  // Specify number of clusters

        var pipeline = _mlContext.Transforms.Conversion.ConvertType(new[] {
                new InputOutputColumnPair("VictimCountFloat", nameof(CrimeDto.VictimCount)),
                new InputOutputColumnPair("ArrestMadeFloat", nameof(CrimeDto.ArrestMade)),
                new InputOutputColumnPair("RecurringIncidentFloat", nameof(CrimeDto.RecurringIncident))
            }, DataKind.Single) // Convert to Single (float)
          .Append(_mlContext.Transforms.Concatenate("Features", "VictimCountFloat", "ArrestMadeFloat", "RecurringIncidentFloat"))
          .Append(_mlContext.Clustering.Trainers.KMeans("Features", numberOfClusters: 3));  // Specify number of clusters


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

    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId { get; set; }
    }

    public class ClusterResult
    {
        public int RecordId { get; set; }
        public uint ClusterId { get; set; }
    }
}