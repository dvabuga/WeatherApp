using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Controllers
{
    public class MainController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly IFaultService _faultService;

        public MainController(IWeatherService weatherService, IFaultService faultService)
        {
            _weatherService = weatherService;
            _faultService = faultService;
        }


        public async Task<IActionResult> ShowChart()
        {
            //var currentForecast = await _weatherService.GetCurrentForecast();
            var previousForecasts = await _weatherService.GetPreviousForecast();

            var model = await _faultService.CalculateFaults(previousForecasts);

            return View("ShowChart", model);
        }
    }
}
