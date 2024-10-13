using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BlazorApp2.Services.Crimes;

[DataContract]
public record CrimeDashboardDto
{
    public Guid? BatchID { get; init; }
    public string? SeverityId { get; set; }
    public string? CrimeTypeId { get; set; }
    public string? PoliceDistrictId { get; set; }
    public string? WeatherConditionId { get; set; }
    public string? CrimeMotiveId { get; set; }
    [Required]
    public int CaseID { get; init; }
    [Display(Name = "Type")]
    [Required]
    public string? CrimeType { get; init; }
    [Required]
    public string? Date { get; init; }
    [Required]
    public TimeSpan Time { get; init; }
    [Required]
    public string? Address { get; init; }
    [Required]
    public string? Latitude { get; init; }
    [Required]
    public string? Longitude { get; init; }
    [Required]
    public string? Severity { get; init; }
    [Required]
    public string? Description { get; init; }
    [Required]
    public string? WeaponUsed { get; init; }
    [Required]
    public int VictimCount { get; init; }
    [Required]
    public string? SuspectDescription { get; init; }
    [Display(Name = "Arrest Made")]
    [Required]
    public int ArrestMade { get; init; }
    public string? ArrestDate { get; init; }
    [Display(Name = "Response Time")]
    [Required]
    public int ResponseTimeInMinutes { get; init; }
    [Required]
    public string? PoliceDistrict { get; init; }
    [Display(Name = "Weather")]
    [Required]
    public string? WeatherCondition { get; init; }
    [Display(Name = "Motive")]
    public string? CrimeMotive { get; init; }
    public string? NearbyLandmarks { get; init; }
    [Required]
    public int RecurringIncident { get; init; }
    public int PopulationDensityPerSqKm { get; init; }
    public string? UnemploymentRate { get; init; }
    public string? MedianIncome { get; init; }
    public string? ProximityToPoliceStationInKm { get; init; }
    public bool StreetLightPresent { get; init; }
    public bool CCTVCoverage { get; init; }
    public bool AlcoholOrDrugInvolvement { get; init; }
    public bool IsNotSanitized()
    {
        if (string.IsNullOrWhiteSpace(Latitude)) return true;
        if (string.IsNullOrWhiteSpace(Longitude)) return true;

        return Latitude == "0" || Longitude == "0";
    }
}
