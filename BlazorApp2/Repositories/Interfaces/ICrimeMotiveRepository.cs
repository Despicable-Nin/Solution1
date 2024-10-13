using BlazorApp2.Data;

namespace BlazorApp2.Repositories.Interfaces;

public interface ICrimeMotiveRepository
{
    Task<IEnumerable<CrimeMotive>> GetCrimeMotivesAsync();
    Task AddCrimeMotivesAsync(IEnumerable<CrimeMotive> crimeMotives);
}

public interface IAddressRepository
{
    Task<Dictionary<string, (double, double)>> GetAddresses();
    Task<Address> CreateAddress(Address address);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
