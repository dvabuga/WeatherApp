using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<JObject> GetCurrentForecast();
        Task<JObject[]> GetPreviousForecast();
    }
}
