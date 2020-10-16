using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public interface IGetForecast
    {
        async Task<IEnumerable<ForecastModel>> GetForecastAsync(IHttpClientFactory httpClientFactory, IEnumerable<string> urls)
        {
            var client = httpClientFactory.CreateClient();
            IEnumerable<Task<JObject>> downloadTasksQuery = urls.Select(url => client.GetJobjectAsync(url));

            //start tasks
            Task<JObject>[] downloadTasks = downloadTasksQuery.ToArray();
            var finishedTasks = await Task.WhenAll(downloadTasks);

            var forecastModels = finishedTasks.Select(c => new ForecastModel()
            {
                Time = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(c["current"]["dt"])).ToLocalTime(),
                Temp = Convert.ToInt64(c["current"]["temp"])
            }).OrderBy(c => c.Time).ToList();

            return forecastModels;
        }


    }


}