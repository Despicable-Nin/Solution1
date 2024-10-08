
using BlazorApp2.Services.Crimes;
using BlazorApp2.Services.Geocoding;
using Serilog;

namespace BlazorApp2.BackgroundService
{
    public class GISBackgroundService(IServiceScopeFactory serviceScopeFactory) : IHostedService, IDisposable
    {
        private Timer? _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {    
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            Log.Logger.Information("This is logged from {ServiceName} - {Timestamp}", nameof(GISBackgroundService.DoWork), DateTime.Now);

            using var scope = serviceScopeFactory.CreateScope();
            var crimeService = scope.ServiceProvider.GetRequiredService<ICrimeService>();
            var geoCodingService = scope.ServiceProvider.GetRequiredService<NominatimGeocodingService>();
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
