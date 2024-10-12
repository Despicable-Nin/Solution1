using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.Services.Clustering
{
    public record SanitizedCrimeRecord
    {
        public SanitizedCrimeRecord() { }

        [Key]
        public int Id { get; init; }
        public string CaseID { get; init; } // Converted from int
        public string CrimeType { get; init; } // Converted from int
        public string Date { get; init; } // Might require specific conversion logic
        public string Time { get; init; } // Might require specific conversion logic
        public string RawAddress { get; init; }
        public string RawDate { get; init; }
        public string RawTime { get; init; }
        public string Latitude { get; init; }
        public string Longitude { get; init; }
        public string SeverityId { get; init; } // Converted from int (assuming a numerical severity scale)
        public string VictimCount { get; init; }
        public string ArrestMade { get; init; } // Converted from int (assuming binary: 0/1)
        public string ArrestDate { get; init; } // Might require specific conversion logic
        public string ResponseTimeInMinutes { get; init; } // Converted from int
        public string PoliceDistrictId { get; init; } // Converted from int (assuming numerical district ID)
        public string WeatherConditionId { get; init; } // Converted from int (assuming a numerical weather code)
        public string CrimeMotiveId { get; init; } // Converted from int (assuming a numerical motive code)

        public string NearbyLandmarkLatitude { get; init; }
        public string NearbyLandmarkLongitude { get; init; }
        public string RecurringIncident { get; init; } // Converted from int (assuming binary: 0/1)
        public string PopulationDensityPerSqKm { get; init; }
        public string UnemploymentRate { get; init; }
        public string MedianIncome { get; init; }
        public string ProximityToPoliceStationInKm { get; init; }
        public string StreetLightPresent { get; init; } // Converted from bool (assuming 1 for present, 0 for absent)
        public string CCTVCoverage { get; init; } // Converted from bool (assuming 1 for covered, 0 for not covered)
        public string AlcoholOrDrugInvolvement { get; init; } // Converted from bool (assuming 1 for involved, 0 for not involved)



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