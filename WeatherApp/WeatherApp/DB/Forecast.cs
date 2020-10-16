using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.DB
{
    public class Forecast
    {
        [Key]
        public Guid Id { get; set; }
        public string CityName { get; set; }

        [DataType("jsonb")]
        public string CurrentForecast { get; set; }

        [DataType("jsonb")]
        public string DailyForecast { get; set; }

        public DateTimeOffset? FaultUpdateLastDate { get; set; }
    }
}
