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
using WeatherApp.Models;
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
                        ws.Column(3 + j).Width = 15;
                        ws.Cell(6 + k, 3 + j).Value = faults.ChartData[i].Item1[j].date;
                    }

                    for (var j = 0; j < faults.ChartData[i].Item1.Count; j++)
                    {
                        ws.Cell(7 + k, 3 + j).Value = faults.ChartData[i].Item1[j].temp;
                    }

                    k += 4;

                }

                workbook.SaveAs(stream);
                stream.Position = 0;
            }

            return stream;
        }

        public async Task UploadFaultsToStorage(Stream faultFile)
        {
            var oauthToken = _configuration.OauthToken;
            var diskApi = new DiskHttpApi(oauthToken);
            var uploadUrl = await diskApi.Files.GetUploadLinkAsync("/Files/faults1.xlsx", true, CancellationToken.None);
            await diskApi.Files.UploadAsync(uploadUrl, faultFile);
        }
    }
}
