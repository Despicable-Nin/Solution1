using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.Data
{

    public class Crime
    {
        public Crime()
        {
            
        }

        public Guid Id { get; set; }
        public DateTime DateUploaded { get; set; } = DateTime.Now;
        public int CaseID { get; set; }
        public string? CrimeType { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string? Address { get; set; }
        public string? Severity { get; set; }
        public string? Description { get; set; }
        public string? WeaponUsed { get; set; }
        public int VictimCount { get; set; }
        public string? SuspectDescription { get; set; }
        public bool ArrestMade { get; set; }
        public DateTime? ArrestDate { get; set; } // Nullable for no arrest
        public int ResponseTimeInMinutes { get; set; }
        public string? PoliceDistrict { get; set; }
        public string? WeatherCondition { get; set; }
        public string? CrimeMotive { get; set; }
        public string? NearbyLandmarks { get; set; }
        public bool RecurringIncident { get; set; }
        public int PopulationDensityPerSqKm { get; set; }
        public double UnemploymentRate { get; set; }
        public decimal MedianIncome { get; set; }
        public double ProximityToPoliceStationInKm { get; set; }
        public bool StreetLightPresent { get; set; }
        public bool CCTVCoverage { get; set; }
        public bool AlcoholOrDrugInvolvement { get; set; }
    }

}
