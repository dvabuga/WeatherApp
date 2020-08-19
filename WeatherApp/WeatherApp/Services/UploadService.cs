using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeatherApp.Models.DB;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services
{
    public class UploadService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly IWeatherService _weatherService;
        private readonly IFaultService _faultService;


        public UploadService(IServiceProvider serviceProvider, IFaultService faultService, IWeatherService weatherService)
        {
            _provider = serviceProvider;
            _weatherService = weatherService;
            _faultService = faultService;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = _provider.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var forecasts = _context.Forecasts.ToList();

                    foreach (var forecast in forecasts)
                    {
                        var lastUpdateTime = (DateTimeOffset)forecast.FaultUpdateLastDate;
                        var d = lastUpdateTime.AddHours(3);
                        var compare = DateTimeOffset.Compare(DateTimeOffset.Now, d);

                        if (compare > 0)
                        {
                            var previousForecasts = await _weatherService.GetPreviousForecast();
                            var model = await _faultService.CalculateFaults(previousForecasts);
                            var fileStream = _faultService.GetFileWithFaults(model);
                            await _faultService.UploadFaultsToStorage(fileStream);
                            fileStream.Dispose();
                        }
                        forecast.FaultUpdateLastDate = DateTimeOffset.Now;
                        _context.Update(forecast);
                        _context.SaveChanges();
                    }
                }
                await Task.Delay(30000);
            }
        }
    }
}
