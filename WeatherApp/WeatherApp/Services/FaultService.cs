using ClosedXML.Excel;
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
                var previousDate = prevForcast["current"]["dt"]; //unixtime
                var formattedPreviousDate = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(previousDate)).ToLocalTime().ToString("MM-dd-yy H:mm:ss");

                var previousTemp = Convert.ToDouble(prevForcast["current"]["temp"]);

                var forecastError = Math.Abs(currentTemp - previousTemp);
                chartModel.ChartData.Add((formattedPreviousDate, forecastError));
            }

            return chartModel;
        }

        public Stream GetFileWithFaults(ChartModel faults)
        {
            var stream = new MemoryStream();

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Sample Sheet");
                ws.Cell("C4").Value = $"Погрешности измерений на {DateTime.Now}";
                ws.Range("C4:G4").Row(1).Merge();
                ws.Cell("B6").Value = "Интервал";
                ws.Cell("B7").Value = "Погрешность";
                ws.Column(2).Width = 20;


                for (var i = 0; i < faults.ChartData.Count; i++)
                {
                    ws.Column(3 + i).Width = 15;
                    ws.Cell(6, 3 + i).Value = faults.ChartData[i].date;
                }

                for (var i = 0; i < faults.ChartData.Count; i++)
                {
                    ws.Cell(7, 3 + i).Value = faults.ChartData[i].temp;
                }

                workbook.SaveAs(stream);
            }

            return stream;
        }
    }
}
