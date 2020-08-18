using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services.Interfaces
{
    public interface IFaultService
    {
        ChartModel CalculateFaults(JObject[] previousForecasts, JObject currentForecast);
        Stream GetFileWithFaults(ChartModel faults);
    }
}
