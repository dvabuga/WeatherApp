using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class ChartModel
    {
        public List<(string date, double temp)> ChartData { get; set; } = new List<(string date, double temp)>();
    }
}
