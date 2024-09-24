namespace BlazorApp2.Services.Crimes;

public interface ICrimeService
{
    public Task<PaginatedCrimesDto> GetCrimesAsync(int page = 1, int pageSize = 10);
    public Task AddCrimesAsync(IEnumerable<CrimeDashboardDto> crimes);
    public Task<IEnumerable<string>> GetCrimeTypes();
    public Task UpsertCrimeMotiveReferences(IEnumerable<string> crimeMotives);
}
