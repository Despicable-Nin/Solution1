using espasyo_domain.Seedwork;

namespace espasyo_domain.AggregatesModel.IncidentAggregate
{
    public interface IIncidentRepository : IRepository<Incident>
    {
        Incident Create(Incident incident);
        Incident Update(Incident incident);
        bool Delete(Incident incident);
        Task<Incident> Get(Guid id);
        IEnumerable<Incident> GetAll(int pageSize, int pageIndex);
        IEnumerable<Incident> GetByMotive(MotiveEnum motive, KeyValuePair<DateTime, DateTime>? dateRange);
        IEnumerable<Incident> GetBySeverity(SeverityEnum severity, KeyValuePair<DateTime, DateTime>? dateRange);
        IEnumerable<Incident> GetByPoliceDistrict(MuntinlupaPoliceDistrictEnum policeDistrict, KeyValuePair<DateTime, DateTime>? dateRange);
        IEnumerable<Incident> GetByWeather(WeatherConditionEnum weatherCondition, KeyValuePair<DateTime, DateTime>? dateRange);
        int GetCount();
    }

   
}
