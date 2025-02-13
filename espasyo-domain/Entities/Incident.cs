namespace espasyo_domain.Entities
{
    public enum SeverityEnum
    {
        Low = 0, Medium = 1, High = 2
    }

    public enum CrimeTypeEnum
    {
        Arson,
        Assault,
        Burglary,
        Corruption,
        Counterfeiting,
        CyberCrime,
        DomesticViolence,
        DrugTrafficking,
        Embezzlement,
        Extortion,
        Fraud,
        HumanTrafficking,
        Homicide,
        IllegalPossessionOfFirearms,
        Kidnapping,
        Murder,
        Rape,
        Robbery,
        Theft,
        Vandalism
    }

    public enum MotiveEnum
    {
        Anger,
        Greed,
        Jealousy,
        PersonalGain,
        Political,
        Revenge,
        Terrorism,
        Unknown,
        Other
    }

    public enum WeatherConditionEnum
    {
        Clear,
        PartlyCloudy,
        Cloudy,
        Overcast,
        Rain,
        Thunderstorm,
        Typhoon,
        TropicalStorm,
        Drizzle,
        Showers,
        Haze,
        Fog,
        Windy
    }

    public enum MuntinlupaPoliceDistrictEnum
    {
        Alabang,
        Bayanan,
        Buli,
        Cupang,
        Poblacion,
        Putatan,
        Sucat,
        Tunasan
    }



    public class Incident
    {
        public Guid Id { get; set; }
        public string? CaseId { get; set; }
        public string? Address { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public SeverityEnum Severity { get; set; }
        public CrimeTypeEnum CrimeType { get; set; }
        public MotiveEnum Motive { get; set; }
        public MuntinlupaPoliceDistrictEnum PoliceDistrict { get; set; }
        public string? OtherMotive { get; set; }
        public WeatherConditionEnum Weather { get; set; } 
        public DateTimeOffset? TimeStamp { get; set; }

    }
}
