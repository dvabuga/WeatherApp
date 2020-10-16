using ClosedXML.Excel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherApp.Files.SharedModels;
using WeatherApp.Services.Interfaces;
using YandexDisk.Client.Http;

namespace WeatherApp.Services
{
    public class FaultService : IFaultService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WeatherServiceSettings _configuration;
        private readonly IGetForecast _getForecast;

        public FaultService(IHttpClientFactory httpClientFactory, IOptions<WeatherServiceSettings> configuration, IGetForecast getForecast) 
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration.Value;
            _getForecast = getForecast;
        }

        public ChartModel CalculateFaults(IEnumerable<ForecastModel> historicalForecast, IEnumerable<IEnumerable<ForecastModel>> intervalForecast)
        {
            var hForecast = historicalForecast as List<ForecastModel>;
            var iForecast = intervalForecast as List<List<ForecastModel>>;

            var chartModel = new ChartModel();
            var intervals = _configuration.Intervals.ToList();

            for (var i = 0; i < iForecast.Count(); i++)
            {
                var f = iForecast[i];
                var chart = new List<(string date, double temp)>();
                for (var j = 0; j < iForecast[i].Count; j++)
                {
                    var temp = Math.Abs(hForecast[j].Temp - f[j].Temp);
                    chart.Add((f[j].Time.ToString("MM-dd-yy H:mm:ss"), temp));
                }

                chartModel.ChartData.Add((chart, intervals[i]));
            }

            return chartModel;
        }
    }
}
