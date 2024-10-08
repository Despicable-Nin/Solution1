using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.Data
{
    public class FlatCluster
    {
        public FlatCluster() { }

        [Key]
        public int Id { get; set; } 
        public string CaseID { get; set; } // Converted from int
        public string CrimeType { get; set; } // Converted from int
        public string Date { get; set; } // Might require specific conversion logic
        public string Time { get; set; } // Might require specific conversion logic
        public string RawAddress { get; set; }
        public string RawDate { get; set; }
        public string RawTime { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Severity { get; set; } // Converted from int (assuming a numerical severity scale)
        public string VictimCount { get; set; }
        public string ArrestMade { get; set; } // Converted from int (assuming binary: 0/1)
        public string ArrestDate { get; set; } // Might require specific conversion logic
        public string ResponseTimeInMinutes { get; set; } // Converted from int
        public string PoliceDistrict { get; set; } // Converted from int (assuming numerical district ID)
        public string WeatherCondition { get; set; } // Converted from int (assuming a numerical weather code)
        public string CrimeMotive { get; set; } // Converted from int (assuming a numerical motive code)

        public string NearbyLandmarkLatitude { get; set; }
        public string NearbyLandmarkLongitude { get; set; }
        public string RecurringIncident { get; set; } // Converted from int (assuming binary: 0/1)
        public string PopulationDensityPerSqKm { get; set; }
        public string UnemploymentRate { get; set; }
        public string MedianIncome { get; set; }
        public string ProximityToPoliceStationInKm { get; set; }
        public string StreetLightPresent { get; set; } // Converted from bool (assuming 1 for present, 0 for absent)
        public string CCTVCoverage { get; set; } // Converted from bool (assuming 1 for covered, 0 for not covered)
        public string AlcoholOrDrugInvolvement { get; set; } // Converted from bool (assuming 1 for involved, 0 for not involved)



        public DateTime GetDate() => GetDate(Date);

        public DateTime GetTime() => GetDate(Time);

        public DateTime GetArrestDate() => GetDate(ArrestDate);

        private static DateTime GetDate(string ticks)
        {
            if (string.IsNullOrEmpty(ticks)) return default;

            var isNotParsed = int.TryParse(ticks, out int intTicks);

            if (isNotParsed) return DateTime.MinValue;

            return new DateTime(intTicks, DateTimeKind.Utc);
        }
    }
}