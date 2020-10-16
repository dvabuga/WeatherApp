using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.Files.SharedModels
{
    public class ChartModel
    {
        public List<(List<(string date, double temp)>, int interval)> ChartData { get; set; } = new List<(List<(string date, double temp)>, int interval)>();
    }
}
