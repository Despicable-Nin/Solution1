namespace BlazorApp2.Data
{
    public class FatCluster
    {
        protected FatCluster()
        {

        }

        public Guid Id { get; set; }
        public int CaseID { get; set; }
        public int CrimeType { get; set; }
        public int Date { get; set; }
        public int Time { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Severity { get; set; }
        public int VictimCount { get; set; }
        public int ArrestMade { get; set; }
        public int ArrestDate { get; set; }
        public int ResponseTimeInMinutes { get; set; }
        public int PoliceDistrict { get; set; }
        public int WeatherCondition { get; set; }
        public int CrimeMotive { get; set; }
       
        public double NearbyLandmarkLatitude { get; set; }
        public double NearbyLandmarkLongitude { get; set; }
        public int RecurringIncident { get; set; }
        public int PopulationDensityPerSqKm { get; set; }
        public double UnemploymentRate { get; set; }
        public decimal MedianIncome { get; set; }
        public double ProximityToPoliceStationInKm { get; set; }
        public bool StreetLightPresent { get; set; }
        public bool CCTVCoverage { get; set; }
        public bool AlcoholOrDrugInvolvement { get; set; }
    }

}
