using BlazorApp2.Data;
using BlazorApp2.Repositories;

namespace BlazorApp2.Services.Crimes
{
    public class CrimeService : ICrimeService
    {
        private readonly ICrimeRepository _crimeRepository;

        public CrimeService(ICrimeRepository crimeRepository)
        {
            _crimeRepository = crimeRepository;
        }

        public async Task AddCrimesAsync(IEnumerable<CrimeDto> crimeDtos)
        {
            var crimes = crimeDtos.Select(c => new Crime
            {
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
                ArrestMade = c.ArrestMade,
                ArrestDate = !string.IsNullOrEmpty(c.ArrestDate) ? DateTime.Parse(c.ArrestDate) : DateTime.MinValue,
                ResponseTimeInMinutes = c.ResponseTimeInMinutes,
                PoliceDistrict = c.PoliceDistrict,
                WeatherCondition = c.WeatherCondition,
                CrimeMotive = c.CrimeMotive,
                NearbyLandmarks = c.NearbyLandmarks,
                RecurringIncident = c.RecurringIncident,
                PopulationDensityPerSqKm = c.PopulationDensityPerSqKm,
                UnemploymentRate = ((double)(!string.IsNullOrEmpty(c.UnemploymentRate) ? decimal.TryParse(c.UnemploymentRate, out _) ? decimal.Parse(c.UnemploymentRate) : 0 : 0)),
                MedianIncome = (decimal)(!string.IsNullOrEmpty(c.MedianIncome) ? decimal.TryParse(c.MedianIncome, out _) ? decimal.Parse(c.MedianIncome) : 0 : 0),
                ProximityToPoliceStationInKm = (!string.IsNullOrEmpty(c.ProximityToPoliceStationInKm) ? double.TryParse(c.ProximityToPoliceStationInKm, out _) ? double.Parse(c.ProximityToPoliceStationInKm) : 0 : 0),
                StreetLightPresent = c.StreetLightPresent,
                CCTVCoverage = c.CCTVCoverage,
                AlcoholOrDrugInvolvement = c.AlcoholOrDrugInvolvement
            });

            await _crimeRepository.AddCrimesAsync(crimes);
        }

        public async Task<PaginatedCrimesDto> GetCrimesAsync(int page = 1, int pageSize = 10)
        {
            var result = await _crimeRepository.GetCrimesAsync(page, pageSize);
            IEnumerable<CrimeDto> crimeDtos = result.Item1?.Select(c => new CrimeDto
            {
                Address = c.Address,
                ArrestDate = c.ArrestDate?.ToString("u"),
                ArrestMade = c.ArrestMade,
                CaseID = c.CaseID,
                CCTVCoverage = c.CCTVCoverage,
                CrimeMotive = c.CrimeMotive,
                CrimeType = c.CrimeType,
                Description = c.Description,
                MedianIncome = c.MedianIncome.ToString(),
                NearbyLandmarks = c.NearbyLandmarks,
                PoliceDistrict = c.PoliceDistrict,
                PopulationDensityPerSqKm = c.PopulationDensityPerSqKm,
                ProximityToPoliceStationInKm = c.ProximityToPoliceStationInKm.ToString(),
                RecurringIncident = c.RecurringIncident,
                ResponseTimeInMinutes = c.ResponseTimeInMinutes,
                Severity = c.Severity,
                StreetLightPresent = c.StreetLightPresent,
                SuspectDescription = c.SuspectDescription,
                Time = c.Time,
                UnemploymentRate = c.UnemploymentRate.ToString(),
                VictimCount = c.VictimCount,
                WeatherCondition = c.WeatherCondition,
                WeaponUsed = c.WeaponUsed,
                AlcoholOrDrugInvolvement = c.AlcoholOrDrugInvolvement
            }) ?? [];
            return new PaginatedCrimesDto(crimeDtos, result.totalCount);
        }

        public async Task<IEnumerable<string>> GetCrimeTypes()
        {
            throw new NotImplementedException();
        }
    }

}
