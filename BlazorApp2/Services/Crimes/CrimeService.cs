using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;

namespace BlazorApp2.Services.Crimes;

public class CrimeService : ICrimeService
{
    private readonly ICrimeRepository _crimeRepository;

    public CrimeService(ICrimeRepository crimeRepository)
    {
        _crimeRepository = crimeRepository;
    }

    public async Task<int> AddCrimesAsync(IEnumerable<CrimeDashboardDto> crimeDtos)
    {
        if (crimeDtos == null) throw new ArgumentException(nameof(crimeDtos));

        // Step 1: Get all existing CaseIDs at once
        var existingCaseIds = await _crimeRepository.GetExistingCaseIdsAsync();

        // Step 2: Use Except to filter out existing CaseIDs from crimeDtos
        var crimes = crimeDtos
            .Where(c => !existingCaseIds.Contains(c.CaseID))
                    .Select(c => new Crime
        {
                        BatchId = c.BatchID,
            CaseID = c.CaseID,
            CrimeType = c.CrimeType,
            Date = DateTime.Parse(c.Date),
            Time = c.Time,
            Address = c.Address,
            Severity = c.Severity,
            Description = c.Description,
            WeaponUsed = c.WeaponUsed,
            VictimCount = c.VictimCount,
            SuspectDescription = c.SuspectDescription,
            ArrestMade = c.ArrestMade == 1,
            ArrestDate = !string.IsNullOrEmpty(c.ArrestDate) ? DateTime.TryParse(c.ArrestDate, out _) ? DateTime.Parse(c.ArrestDate) : DateTime.MinValue : DateTime.MinValue,
            ResponseTimeInMinutes = c.ResponseTimeInMinutes,
            PoliceDistrict = c.PoliceDistrict,
            WeatherCondition = c.WeatherCondition,
            CrimeMotive = c.CrimeMotive,
            NearbyLandmarks = c.NearbyLandmarks,
            RecurringIncident = c.RecurringIncident == 1,
            PopulationDensityPerSqKm = c.PopulationDensityPerSqKm,
            UnemploymentRate = ((double)(!string.IsNullOrEmpty(c.UnemploymentRate) ? decimal.TryParse(c.UnemploymentRate, out _) ? decimal.Parse(c.UnemploymentRate) : 0 : 0)),
            MedianIncome = (decimal)(!string.IsNullOrEmpty(c.MedianIncome) ? decimal.TryParse(c.MedianIncome, out _) ? decimal.Parse(c.MedianIncome) : 0 : 0),
            ProximityToPoliceStationInKm = (!string.IsNullOrEmpty(c.ProximityToPoliceStationInKm) ? double.TryParse(c.ProximityToPoliceStationInKm, out _) ? double.Parse(c.ProximityToPoliceStationInKm) : 0 : 0),
            StreetLightPresent = c.StreetLightPresent,
            CCTVCoverage = c.CCTVCoverage,
            AlcoholOrDrugInvolvement = c.AlcoholOrDrugInvolvement
        });

        int affectedRows = 0;
        if (crimes.Any())
        {
           affectedRows = await _crimeRepository.AddCrimesAsync(crimes);
        }

        return affectedRows;
    }

    public bool AreAllSanitized(IEnumerable<CrimeDashboardDto> dtos)
    {
        return dtos.Any(i => !i.IsWithoutLatLong);
    }

    public async Task<PaginatedCrimesDto> GetCrimesAsync(int page = 1, int pageSize = 10)
    {
        var result = await _crimeRepository.GetCrimesAsync(page, pageSize);
        IEnumerable<CrimeDashboardDto> crimeDtos = result.Item1?.Select(c => new CrimeDashboardDto
        {
            Address = c.Address,
            Latitude = c.Latitude.HasValue ? c.Latitude.ToString() : "0",
            Longitude = c.Longitude.HasValue ? c.Longitude.ToString() : "0",
            IsWithoutLatLong = c.Latitude.HasValue && c.Longitude.HasValue,  
            ArrestDate = c.ArrestDate?.ToString("u"),
            ArrestMade = c.ArrestMade ? 1 : 0,
            CaseID = c.CaseID,
            CCTVCoverage = c.CCTVCoverage,
            CrimeMotive = c.CrimeMotive,
            CrimeType = c.CrimeType,
            CrimeMotiveId = c.CrimeMotiveId,
            CrimeTypeId = c.CrimeTypeId,
            Date = c.Date.ToString("u"),
            Description = c.Description,
            MedianIncome = c.MedianIncome.ToString(),
            NearbyLandmarks = c.NearbyLandmarks,
            PoliceDistrict = c.PoliceDistrict,
            PopulationDensityPerSqKm = c.PopulationDensityPerSqKm,
            ProximityToPoliceStationInKm = c.ProximityToPoliceStationInKm.ToString(),
            RecurringIncident = c.RecurringIncident ? 1 : 0,
            ResponseTimeInMinutes = c.ResponseTimeInMinutes,
            Severity = c.Severity,
            SeverityId = c.SeverityId,
            StreetLightPresent = c.StreetLightPresent,
            SuspectDescription = c.SuspectDescription,
            Time = c.Time,
            UnemploymentRate = c.UnemploymentRate.ToString(),
            VictimCount = c.VictimCount,
            WeatherCondition = c.WeatherCondition,
            WeatherConditionId = c.WeatherConditionId,
            WeaponUsed = c.WeaponUsed,
            AlcoholOrDrugInvolvement = c.AlcoholOrDrugInvolvement
        }) ?? [];
        return new PaginatedCrimesDto(crimeDtos, result.totalCount);
    }

    public async Task<IEnumerable<string>> GetCrimeTypes() => throw new NotImplementedException();

    public Task UpsertCrimeMotiveReferences(IEnumerable<string> crimeMotives) => throw new NotImplementedException();
}
