using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeatherApp.Models;
using WeatherApp.Models.DB;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Controllers
{
    public class MainController : Controller
    {
        private readonly IForecastService _weatherService;
        private readonly IFaultService _faultService;
        private readonly ApplicationDbContext _context;

        public MainController(IForecastService weatherService, IFaultService faultService, ApplicationDbContext context)
        {
            _weatherService = weatherService;
            _faultService = faultService;
            _context = context;
        }


        public async Task<IActionResult> ShowChart()
        {
            var currentForecast = await _weatherService.GetCurrentForecast();

            var current = JsonConvert.SerializeObject(currentForecast["current"]);
            var daily = JsonConvert.SerializeObject(currentForecast["daily"]);

            _context.Forecasts.AsQueryable<Forecast>().AddOrUpdateByCityName(_context, current, daily);

            var historicalForecast = await _weatherService.GetHistoricalForecast();
            var intervalForecast = await _weatherService.GetIntervalForecast(historicalForecast);

            var model = _faultService.CalculateFaults(historicalForecast, intervalForecast);

            return View("ShowChart", model);
        }
    }
}
