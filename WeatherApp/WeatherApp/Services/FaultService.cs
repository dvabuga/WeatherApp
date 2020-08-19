using ClosedXML.Excel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services.Interfaces;
using YandexDisk.Client.Http;

namespace WeatherApp.Services
{
    public class FaultService : IFaultService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FaultService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        async Task<JObject> ProcessURL(string url, HttpClient client)
        {
            var response = await client.GetAsync(url);
            var urlContents = await response.Content.ReadAsStringAsync();
            var dataObj = JsonConvert.DeserializeObject<JObject>(urlContents);
            return dataObj;
        }

        //private long GetDates(JObject forecast, int i)
        //{
        //    var date = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(forecast["current"]["dt"])).ToLocalTime().ToUnixTimeSeconds();
        //    var d = date - i * 60 * 60;
        //    return d;
        //}

        public async Task<ChartModel> CalculateFaults(List<ForecastModel> previousForecasts)
        {
            var client = _httpClientFactory.CreateClient();

            var openWeatherUrl = _configuration["WeatherServiceSettings:OpenWeatherApiUrl"];
            var lon = _configuration["WeatherServiceSettings:CityCoords:Longitude"];
            var lat = _configuration["WeatherServiceSettings:CityCoords:Latitude"];
            var appId = _configuration["WeatherServiceSettings:OpenWeatherAppId"];

            var chartModel = new ChartModel();
            var intervals = new[] { 1, 6 };

            for (var i = 0; i < intervals.Length; i++)
            {
                //for (var j = 0; j < previousForecasts.Length - 1; j++)
                //{
                //}

                var datesUnixTime = previousForecasts.Select(forecast => forecast.Time.ToUnixTimeSeconds() - intervals[i] * 60 * 60).ToList();
                var urls = datesUnixTime.Select(time => $"{openWeatherUrl}onecall/timemachine?lat={lat}&lon={lon}&dt={time}&appid={appId}&units=metric&lang=ru").ToList();
                IEnumerable<Task<JObject>> downloadTasksQuery = from url in urls
                                                                select ProcessURL(url, client);
                Task<JObject>[] downloadTasks = downloadTasksQuery.ToArray();
                var finishedTasks = await Task.WhenAll(downloadTasks);

                var f = finishedTasks.Select(c => new ForecastModel()
                {
                    Time = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(c["current"]["dt"])).ToLocalTime(),
                    Temp = Convert.ToInt64(c["current"]["temp"])
                }).OrderBy(c => c.Time).ToList();

                var chart = new List<(string date, double temp)>();
                for (var j = 0; j < f.Count; j++)
                {
                    var temp = Math.Abs(previousForecasts[j].Temp - f[j].Temp);
                    chart.Add((f[j].Time.ToString("MM-dd-yy H:mm:ss"), temp));

                    //var tt = f.Where(c => prev.Time.Hour - c.Time.Hour == intervals[i]).FirstOrDefault();
                    //var temp = Math.Abs(prev.Temp - tt.Temp);
                }
                //foreach (var prev in previousForecasts)
                //{
                //    var tt = f.Where(c => prev.Time.Hour - c.Time.Hour == intervals[i]).FirstOrDefault();
                //    var temp = Math.Abs(prev.Temp - tt.Temp);
                //}
                chartModel.ChartData.Add((chart, intervals[i]));

            }

            // var currentTemp = Convert.ToDouble(currentForecast["current"]["temp"]);

            //foreach (var prevForcast in previousForecasts)
            //{
            //    var previousDate = prevForcast["current"]["dt"]; //unixtime
            //    var formattedPreviousDate = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(previousDate)).ToLocalTime().ToString("MM-dd-yy H:mm:ss");

            //    var previousTemp = Convert.ToDouble(prevForcast["current"]["temp"]);

            //    var forecastError = Math.Abs(currentTemp - previousTemp);
            //    chartModel.ChartData.Add((formattedPreviousDate, forecastError));
            //}

            return chartModel;
        }

        public Stream GetFileWithFaults(ChartModel faults)
        {
            var stream = new MemoryStream();

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Sample Sheet");

                ws.Column(2).Width = 20;
                var k = 0;

                for (var i = 0; i < faults.ChartData.Count; i++)
                {
                    ws.Cell("C4").Value = $"Погрешности измерений на {DateTime.Now}";
                    ws.Range("C4:G4").Row(1).Merge();

                    ws.Cell($"B{5 + k}").Value = $"Интервал - {faults.ChartData[i].interval}";
                    ws.Cell($"B{6 + k}").Value = "Интервал";
                    ws.Cell($"B{7 + k}").Value = "Погрешность";

                    for (var j = 0; j < faults.ChartData[i].Item1.Count; j++)
                    {
                        ws.Column(3 + i).Width = 15;
                        ws.Cell(6 + k, 3 + i).Value = faults.ChartData[i].Item1[j].date;
                    }

                    for (var j = 0; j < faults.ChartData[i].Item1.Count; j++)
                    {
                        ws.Cell(7 + k, 3 + i).Value = faults.ChartData[i].Item1[j].temp;
                    }

                    k += 4;

                }

                //foreach (var faultChart in faults.ChartData)
                //{
                //    for (var i = 0; i < faultChart.Item1.Count; i++)
                //    {
                //        ws.Column(3 + i).Width = 15;
                //        ws.Cell(6, 3 + i).Value = faultChart.Item1[i].date;
                //    }
                //}




                //for (var i = 0; i < faults.ChartData.Count; i++)
                //{
                //    ws.Cell(7, 3 + i).Value = faults.ChartData[i].temp;
                //}

                //workbook.SaveAs("filse1.xlsx");
                workbook.SaveAs(stream);
                stream.Position = 0;
            }

            return stream;
        }

        public async Task UploadFaultsToStorage(Stream faultFile)
        {
            var oauthToken = _configuration["OauthToken"];
            var diskApi = new DiskHttpApi(oauthToken);
            var uploadUrl = await diskApi.Files.GetUploadLinkAsync("/Files/file.xlsx", true, CancellationToken.None);
            await diskApi.Files.UploadAsync(uploadUrl, faultFile);
        }
    }
}
