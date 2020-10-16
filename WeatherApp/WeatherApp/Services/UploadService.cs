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
        private readonly IForecastService _weatherService;
        private readonly IFaultService _faultService;


        public UploadService(IServiceProvider serviceProvider, IFaultService faultService, IForecastService weatherService)
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
                        if (forecast.FaultUpdateLastDate == null)
                        {
                            await StartUpload(forecast);
                        }
                        else
                        {
                            var lastUpdateTime = (DateTimeOffset)forecast.FaultUpdateLastDate;
                            var d = lastUpdateTime.AddHours(3);
                            var compare = DateTimeOffset.Compare(DateTimeOffset.Now, d);
                            if (compare > 0)
                            {
                                await StartUpload(forecast);
                            }
                        }

                        _context.Update(forecast);
                        _context.SaveChanges();
                    }
                }
                await Task.Delay(30000);
            }
        }

        private async Task StartUpload(Forecast forecast)
        {
            var historicalForecast = await _weatherService.GetHistoricalForecast();
            var intervalForecast = await _weatherService.GetIntervalForecast(historicalForecast);

            var model = _faultService.CalculateFaults(historicalForecast, intervalForecast);

            var fileStream = _faultService.GetFileWithFaults(model);
            try
            {
                await _faultService.UploadFaultsToStorage(fileStream);
                forecast.FaultUpdateLastDate = DateTimeOffset.Now;
            }
            catch (Exception ex)
            {
                //if updating document is opened by user in browser, it will cause I.O block exception
            }
            finally
            {
                fileStream.Dispose();
            }
        }
    }
}
