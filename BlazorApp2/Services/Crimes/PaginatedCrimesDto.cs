namespace BlazorApp2.Services.Crimes
{
    public record PaginatedCrimesDto(IEnumerable<CrimeDto> Crimes, int TotalCount);

}
