using BlazorApp2.Data;

namespace BlazorApp2.Repositories.Interfaces;

public interface IPoliceDistrictRepository
{
    Task<IEnumerable<PoliceDistrict>> GetPoliceDistrictsAsync();
    Task AddPoliceDistrictsAsync(IEnumerable<PoliceDistrict> policeDistricts);
}
