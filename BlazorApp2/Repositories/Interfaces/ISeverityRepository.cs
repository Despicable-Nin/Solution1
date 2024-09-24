using BlazorApp2.Data;

namespace BlazorApp2.Repositories.Interfaces;

public interface ISeverityRepository
{
    Task<IEnumerable<Severity>> GetSeveritiesAsync();
}
