﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<JObject> GetCurrentForecast();
        Task<List<ForecastModel>> GetPreviousForecast();
    }
}
