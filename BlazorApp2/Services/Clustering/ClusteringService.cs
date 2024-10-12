using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using BlazorApp2.Services.Crimes;
using Microsoft.ML;
using Microsoft.ML.Data;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlazorApp2.Services.Clustering;
public partial class ClusteringService() : IClusteringService
{
    private readonly MLContext _mlContext = new();

    public List<ClusterResult> PerformKMeansClustering(IEnumerable<SanitizedCrimeRecord> data, string[] features, int numberOfClusters = 3)
    {
        Log.Logger.Information("PerformKMeansClustering", data, features, numberOfClusters);

        // Convert the input data into an IDataView (ML.NET data structure)
        var schema = SchemaDefinition.Create(typeof(SanitizedCrimeRecord));

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

    public SanitizedCrimeRecord ToSanitizedCrimeRecord(CrimeDashboardDto crime)
    {
        var date = DateTime.Parse(crime.Date ?? DateTime.MinValue.ToString());
        var sanitizedRecord = new SanitizedCrimeRecord
        {
            NearbyLandmarkLatitude = "0",
            NearbyLandmarkLongitude = "0",
            CaseID = crime.CaseID.ToString(),
            CrimeType = crime.CrimeTypeId ?? "0",
            Date = date.Ticks.ToString(),
            Time = crime.Time.Ticks.ToString(),
            Latitude = crime.Latitude ?? "0",
            Longitude = crime.Longitude ?? "0", // Corrected Longitude assignment
            SeverityId = crime.SeverityId ?? "0",
            VictimCount = crime.VictimCount.ToString(),
            ArrestMade = crime.ArrestMade.ToString(),
            ArrestDate = crime.ArrestDate ?? DateTime.MinValue.ToString(),
            ResponseTimeInMinutes = crime.ResponseTimeInMinutes.ToString(),
            PoliceDistrictId = crime.PoliceDistrictId ?? "0",
            WeatherConditionId = crime.WeatherConditionId ?? "0",
            CrimeMotiveId = crime.CrimeMotiveId ?? "0",
            RecurringIncident = crime.RecurringIncident.ToString(),
            PopulationDensityPerSqKm = crime.PopulationDensityPerSqKm.ToString(),
            UnemploymentRate = crime.UnemploymentRate ?? "0",
            MedianIncome = crime.MedianIncome ?? "0",
            ProximityToPoliceStationInKm = crime.ProximityToPoliceStationInKm ?? "0",
            StreetLightPresent = (crime.StreetLightPresent ? 1 : 0).ToString(),
            CCTVCoverage = (crime.CCTVCoverage ? 1 : 0).ToString(),
            AlcoholOrDrugInvolvement = (crime.AlcoholOrDrugInvolvement ? 1 : 0).ToString(),
        };

        return sanitizedRecord;
    }


}