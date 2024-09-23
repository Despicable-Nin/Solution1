using BlazorApp2.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories
{
    public interface ICrimeRepository
    {
        Task<(IEnumerable<Crime>, int totalCount)> GetCrimesAsync(int page, int pageSize);
        Task<IEnumerable<Crime>> GetCrimesAsync();
        Task<Crime> GetCrimeAsync(Guid id);


        Task AddCrimesAsync(IEnumerable<Crime> crime);
        Task<Crime> UpdateCrimeAsync(Crime crime);
        Task DeleteCrimeAsync(Guid id);
    }

    public interface ICrimeMotiveRepository
    {
        Task<IEnumerable<CrimeMotive>> GetCrimeMotivesAsync();
    }

    public interface IWeatherRepository
    {
        Task<IEnumerable<Weather>> GetWeatherConditionsAsync();
    }

    public class WeatherConRepository(ApplicationDbContext context)
    {
        public async Task<IEnumerable<Weather>> GetWeathersAsync()
        {
            return await context.WeatherConditions.AsNoTracking().ToArrayAsync();
        }
    }

        public interface IPoliceDistrictRepository
    {
        Task<IEnumerable<PoliceDistrict>> GetPoliceDistrictsAsync();
    }

    public interface ISeverityRepository
    {
        Task<IEnumerable<Severity>> GetSeveritiesAsync();
    }

    public interface ICrimeTypeRepository
    {
        Task<IEnumerable<CrimeType>> GetCrimeTypesAsync();
    }

}
