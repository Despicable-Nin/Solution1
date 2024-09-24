namespace BlazorApp2.Services.Crimes;

public record PaginatedCrimesDto(IEnumerable<CrimeDashboardDto> Crimes, int TotalCount);
