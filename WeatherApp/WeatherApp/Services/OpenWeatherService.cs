using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Files.SharedModels;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services
{
    public class OpenWeatherService : IForecastService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WeatherServiceSettings _configuration;
        private readonly IGetForecast _getForecast;

        public OpenWeatherService(IHttpClientFactory httpClientFactory, IOptions<WeatherServiceSettings> configuration, IGetForecast getForecast)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration.Value;
            _getForecast = getForecast;
        }

        public async Task<JObject> GetCurrentForecast()
        {
            var url = $"{_configuration.OpenWeatherApiUrl}onecall?lat={_configuration.CityCoords.Latitude}&lon={_configuration.CityCoords.Longitude}&exclude=minutely&appid={_configuration.OpenWeatherAppId}&units=metric&lang=ru";
            var client = _httpClientFactory.CreateClient();
            var dataObj = await client.GetJobjectAsync(url);

            return dataObj;
        }


        public async Task<IEnumerable<ForecastModel>> GetHistoricalForecast()
        {
            var client = _httpClientFactory.CreateClient();
            var timePointsOfMesuareInterval = GetIntervalTimePoints();
            var urls = timePointsOfMesuareInterval.Select(time => $"{_configuration.OpenWeatherApiUrl}onecall/timemachine?lat={_configuration.CityCoords.Latitude}&lon={_configuration.CityCoords.Longitude}&dt={time}&appid={_configuration.OpenWeatherAppId}&units=metric&lang=ru").ToList();

            return await _getForecast.GetForecastAsync(_httpClientFactory, urls);
        }

        public async Task<List<List<ForecastModel>>> GetIntervalForecast(IEnumerable<ForecastModel> historicalForecast)
        {
            var intervals = _configuration.Intervals.ToList();
            var intervalForecast = new List<List<ForecastModel>>();

            for (var i = 0; i < intervals.Count(); i++)
            {
                var datesUnixTime = historicalForecast.Select(forecast => forecast.Time.ToUnixTimeSeconds() - intervals[i] * 60 * 60).ToList();
                var urls = datesUnixTime.Select(time => $"{_configuration.OpenWeatherApiUrl}onecall/timemachine?lat={_configuration.CityCoords.Latitude}&lon={_configuration.CityCoords.Longitude}&dt={time}&appid={_configuration.OpenWeatherAppId}&units=metric&lang=ru").ToList();
                intervalForecast.Add(await _getForecast.GetForecastAsync(_httpClientFactory, urls) as List<ForecastModel>);
            }

            return intervalForecast;
        }


        private List<long> GetIntervalTimePoints()
        {
            var mesuareInterval = DateTime.Now.AddDays(-1).Date; //интервал на котором вычисляем погрешность - предыдущий день
            var timePointsOfMesuareInterval = new List<long>();

            for (var i = 0; i < 24; i += 2)
            {
                var date = (DateTimeOffset)mesuareInterval.AddHours(i);
                timePointsOfMesuareInterval.Add(date.ToUnixTimeSeconds());
            }
            return timePointsOfMesuareInterval;
        }

        // async Task<JObject> ProcessURL(string url, HttpClient client)
        // {
        //     var response = await client.GetAsync(url);
        //     var urlContents = await response.Content.ReadAsStringAsync();
        //     var dataObj = JsonConvert.DeserializeObject<JObject>(urlContents);
        //     return dataObj;
        // }
    }
}
