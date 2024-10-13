using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories;

public class AddressRepository(ApplicationDbContext dbContext) : IAddressRepository
{
    public Task<Address> CreateAddress(Address address)
    {
        dbContext.Add(address);
        return Task.FromResult(address);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken){
        return await dbContext.SaveChangesAsync(cancellationToken);
    } 

  public async Task<Dictionary<string, (double, double)>> GetAddresses()
    {
        var addresses = await dbContext.Addresses
            .Where(a => a.Description != null && a.Latitude.HasValue && a.Longitude.HasValue)
            .ToDictionaryAsync(a => a.Description!, a => (a.Latitude!.Value, a.Longitude!.Value));
        return addresses ?? [];
    }
}
