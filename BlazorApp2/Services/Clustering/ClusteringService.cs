using BlazorApp2.Services.Crimes;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Generic;
using System.Linq;

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
        var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(CrimeDto.Date), nameof(CrimeDto.Severity), nameof(CrimeDto.Address))//, nameof(CrimeData.LocationY))
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
            ClusterId = p.PredictedClusterId
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