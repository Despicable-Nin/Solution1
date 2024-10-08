
using BlazorApp2.Services.Crimes;
using BlazorApp2.Services.Enumerations;
using Serilog;
using System.ComponentModel;
using System.Linq;

namespace BlazorApp2.BackgroundService
{
    public class UploadBackgroundService(IServiceScopeFactory serviceScopeFactory) : IHostedService, IDisposable
    {

        private Timer? _timer;
        private int _count = 0;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            Log.Logger.Information("This is logged from {ServiceName} - {Timestamp}", nameof(UploadBackgroundService.StartAsync), DateTime.Now);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Logger.Information("This is logged from {ServiceName} - {Timestamp}", nameof(UploadBackgroundService.StopAsync), DateTime.Now);
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            // Fetch GIS data
            Log.Logger.Information("This is logged from {ServiceName} - {Timestamp}", nameof(UploadBackgroundService.DoWork), DateTime.Now);
            using var scope = serviceScopeFactory.CreateScope();
            var crimeService = scope.ServiceProvider.GetRequiredService<ICrimeService>();
            var enumService = scope.ServiceProvider.GetRequiredService<IEnumeration>();

            var result = await crimeService.GetCrimesAsync(1, int.MaxValue);


            if (result == null || !result.Crimes.Any())
            {
                Console.WriteLine("No crimes to process.");
                return;

            }

            int currentCount = result.TotalCount;

            if (currentCount == _count)
            {
                Log.Logger.Information("No change in Crime Count.. therefore.. just go on with your life..");
                return;
            }

            var crimes = result.Crimes;

            await PersistEnumerationsAsync(enumService, crimes);

        }

        private static async Task PersistEnumerationsAsync(IEnumeration enumService, IEnumerable<CrimeDashboardDto> crimes)
        {
            try
            {
                // add crimeTypes before saving
                await enumService.AddCrimeTypes(crimes.Select(c => c.CrimeType).Distinct()!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
            }

            try
            {
                // add crimeMotive before saving
                await enumService.AddCrimeMotives(crimes.Select(c => c.CrimeMotive).Distinct()!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
            }

            try
            {
                // add weatherConditions before saving
                await enumService.AddWeatherConditions(crimes.Select(c => c.WeatherCondition).Distinct()!);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
            }

            try
            {

                // add policeDistricts before saving
                await enumService.AddPoliceDistricts(crimes.Select(c => c.PoliceDistrict).Distinct()!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
            }

            try
            {

                // add severityLevels before saving
                await enumService.AddSeverities(crimes.Select(c => c.Severity).Distinct()!);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
            }
        }


        #region IDisposable
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
