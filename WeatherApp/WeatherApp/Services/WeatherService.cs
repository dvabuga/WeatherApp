﻿using Microsoft.Extensions.Configuration;
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
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WeatherServiceSettings _configuration;

        public WeatherService(IHttpClientFactory httpClientFactory, IOptions<WeatherServiceSettings> configuration)
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
            //var current = dataObj["current"];
            //var daily = dataObj["daily"];
            return dataObj;
        }


        public async Task<List<ForecastModel>> GetPreviousForecast()
        {
            var client = _httpClientFactory.CreateClient();
            var intervals = _configuration.Intervals; //интервалы вычисления погрешности
            var lastDay = DateTime.Now.AddDays(-1).Date; //интервал на котором вычисляем погрешность - предыдущий день
            var hoursOfLastDayUnixTime = new List<long>();

            for (var i = 0; i < 24;)
            {
                var date = (DateTimeOffset)lastDay.AddHours(i);
                hoursOfLastDayUnixTime.Add(date.ToUnixTimeSeconds());
                i += 2;
            }

            //var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            //var datesInUnixTime = intervals.Select(interval => DateTimeOffset.Now.ToUnixTimeSeconds() - interval * 60 * 60).ToList();
            var urls = hoursOfLastDayUnixTime.Select(time => $"{_configuration.OpenWeatherApiUrl}onecall/timemachine?lat={_configuration.CityCoords.Latitude}&lon={_configuration.CityCoords.Longitude}&dt={time}&appid={_configuration.OpenWeatherAppId}&units=metric&lang=ru").ToList();

            IEnumerable<Task<JObject>> downloadTasksQuery = from url in urls
                                                            select ProcessURL(url, client); //urls.Select(url => ProcessURL(url, client));
            Task<JObject>[] downloadTasks = downloadTasksQuery.ToArray();
            var finishedTasks = await Task.WhenAll(downloadTasks);


            var f = finishedTasks.Select(c => new ForecastModel()
            {
                Time = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(c["current"]["dt"])).ToLocalTime(),
                Temp = Convert.ToInt64(c["current"]["temp"])
            }).OrderBy(c => c.Time).ToList();
   

            return f;
        }

       



        async Task<JObject> ProcessURL(string url, HttpClient client)
        {
            var response = await client.GetAsync(url);
            var urlContents = await response.Content.ReadAsStringAsync();
            var dataObj = JsonConvert.DeserializeObject<JObject>(urlContents);
            return dataObj;
        }
    }
}
