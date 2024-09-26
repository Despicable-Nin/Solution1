using Microsoft.ML.Data;

namespace BlazorApp2.Services.Clustering;
public partial class ClusteringService
{
    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId { get; set; }
    }
}