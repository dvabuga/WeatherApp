using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services.Interfaces
{
    public interface IForecastService
    {
        Task<JObject> GetCurrentForecast();
        Task<IEnumerable<ForecastModel>> GetHistoricalForecast();
        Task<List<List<ForecastModel>>> GetIntervalForecast(IEnumerable<ForecastModel> historicalForecast);
    }
}
