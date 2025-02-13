using espasyo_domain.Seedwork;

namespace espasyo_domain.AggregatesModel.IncidentAggregate
{

    public class Incident : Entity, IAggregateRoot
    {

        private float? _latitude;
        private float? _longitude;

        public string? CaseId { get; set; }
        public string? Address { get; set; }
        
        public SeverityEnum Severity { get; set; }
        public CrimeTypeEnum CrimeType { get; set; }
        public MotiveEnum Motive { get; set; }
        public MuntinlupaPoliceDistrictEnum PoliceDistrict { get; set; }
        public string? OtherMotive { get; set; }
        public WeatherConditionEnum Weather { get; set; }
        public DateTimeOffset? TimeStamp { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; } = false;

        public void SaveLatLong(float? lat = 0, float? lng = 0)
        {
            _latitude = lat;
            _longitude = lng;
        }

        public float? GetLatitude() => _latitude;
        public float? GetLongitude() => _longitude;

    }
}
