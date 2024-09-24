using BlazorApp2.Data;

namespace BlazorApp2.Repositories.Interfaces;

public interface IPoliceDistrictRepository
{
    Task<IEnumerable<PoliceDistrict>> GetPoliceDistrictsAsync();
}
