using BlazorApp2.Data;

namespace BlazorApp2.Repositories.Interfaces;

public interface ICrimeTypeRepository
{
    Task<IEnumerable<CrimeType>> GetCrimeTypesAsync();
}
