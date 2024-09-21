using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BlazorApp2.Services.Crimes
{
    [DataContract]
    public record CrimeDto
    {
        public int CaseID { get; init; }
        [Display(Name = "Type")]
        public string? CrimeType { get; init; }
        public string? Date { get; init; }
        public TimeSpan Time { get; init; }
        public string? Address { get; init; }
        public string? Severity { get; init; }
        public string? Description { get; init; }
        public string? WeaponUsed { get; init; }
        public int VictimCount { get; init; }
        public string? SuspectDescription { get; init; }
        [Display(Name = "Arrest Made")]
        public bool ArrestMade { get; init; }
        public string? ArrestDate { get; init; }
        [Display(Name = "Response Time")]
        public int ResponseTimeInMinutes { get; init; }
        public string? PoliceDistrict { get; init; }
        [Display(Name = "Weather")]
        public string? WeatherCondition { get; init; }
        [Display(Name = "Motive")]
        public string? CrimeMotive { get; init; }
        public string? NearbyLandmarks { get; init; }

        public bool RecurringIncident { get; init; }
        public int PopulationDensityPerSqKm { get; init; }
        public string? UnemploymentRate { get; init; }
        public string? MedianIncome { get; init; }
        public string? ProximityToPoliceStationInKm { get; init; }
        public bool StreetLightPresent { get; init; }
        public bool CCTVCoverage { get; init; }
        public bool AlcoholOrDrugInvolvement { get; init; }
    }

}
