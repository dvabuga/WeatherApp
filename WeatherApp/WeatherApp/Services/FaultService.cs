using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services
{
    public class FaultService : IFaultService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FaultService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public ChartModel CalculateFaults(JObject[] previousForecasts, JObject currentForecast)
        {
            var chartModel = new ChartModel();
            var currentTemp = Convert.ToDouble(currentForecast["current"]["temp"]);

            foreach (var prevForcast in previousForecasts)
            {
                var previousDate = prevForcast["current"]["dt"];
                var formattedPreviousDate = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(previousDate)).ToLocalTime().ToString("MM-dd-yy H:mm:ss");

                var previousTemp = Convert.ToDouble(prevForcast["current"]["temp"]);

                var forecastError = Math.Abs(currentTemp - previousTemp);
                chartModel.ChartData.Add((formattedPreviousDate, forecastError));
            }

            return chartModel;
        }

        public Stream GetFileWithFaults(ChartModel faults)
        {
            throw new NotImplementedException();
        }
    }
}
