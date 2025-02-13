using espasyo_domain.Entities;

namespace espasyo_domain.Repositories
{
    public interface IIncidentRepository
    {
        Task<Incident> Create(Incident incident);
        Task<Incident> Update(Incident incident);
        Task Delete(Incident incident);
        Task<Incident> Get(Guid id);
        IEnumerable<Incident> GetAll(int pageSize, int pageIndex);
        IEnumerable<Incident> GetByMotive(MotiveEnum motive, KeyValuePair<DateTime, DateTime>? dateRange);
        IEnumerable<Incident> GetBySeverity(SeverityEnum severity, KeyValuePair<DateTime, DateTime>? dateRange);
        IEnumerable<Incident> GetByPoliceDistrict(MuntinlupaPoliceDistrictEnum policeDistrict, KeyValuePair<DateTime, DateTime>? dateRange);
        IEnumerable<Incident> GetByWeather(WeatherConditionEnum weatherCondition,  KeyValuePair<DateTime, DateTime>? dateRange);
        int GetCount();
    }
}
