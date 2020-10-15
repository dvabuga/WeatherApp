using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Services
{
    public class OpenWeatherService : WeatherBase, IForecastService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WeatherServiceSettings _configuration;

        public OpenWeatherService(IHttpClientFactory httpClientFactory, IOptions<WeatherServiceSettings> configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration.Value;
        }

        public async Task<JObject> GetCurrentForecast()
        {
            var client = _httpClientFactory.CreateClient();
            var httpResponse = await client.GetAsync($"{_configuration.OpenWeatherApiUrl}onecall?lat={_configuration.CityCoords.Latitude}&lon={_configuration.CityCoords.Longitude}&exclude=minutely&appid={_configuration.OpenWeatherAppId}&units=metric&lang=ru");
            var content = await httpResponse.Content.ReadAsStringAsync();
            var dataObj = JsonConvert.DeserializeObject<JObject>(content);

            return dataObj;
        }


        public async Task<IEnumerable<ForecastModel>> GetIntervalForecast()
        {
            var client = _httpClientFactory.CreateClient();
            var mesuareInterval = DateTime.Now.AddDays(-1).Date; //интервал на котором вычисляем погрешность - предыдущий день
            var timePointsOfMesuareInterval = new List<long>();

            //get timePoints of mesuare interval
            for (var i = 0; i < 24;)
            {
                var date = (DateTimeOffset)mesuareInterval.AddHours(i);
                timePointsOfMesuareInterval.Add(date.ToUnixTimeSeconds());
                i += 2;
            }

            var urls = timePointsOfMesuareInterval.Select(time => $"{_configuration.OpenWeatherApiUrl}onecall/timemachine?lat={_configuration.CityCoords.Latitude}&lon={_configuration.CityCoords.Longitude}&dt={time}&appid={_configuration.OpenWeatherAppId}&units=metric&lang=ru").ToList();

           return await GetForecastAsync(_httpClientFactory, urls);
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
