namespace BlazorApp2.Services.Clustering;

public class ClusterResult
{
    public uint ClusterId { get; set; }
    public string Latitude { get; set; } = string.Empty;
    public string Longitude { get; set; } = string.Empty;
    public int Count { get; internal set; }
}
