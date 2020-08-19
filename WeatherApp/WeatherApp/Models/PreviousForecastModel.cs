using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class ForecastModel
    {
        public DateTimeOffset Time { get; set; }
        public double Temp { get; set; }
    }
}
