using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.Files.SharedModels
{
    public class WeatherServiceSettings
    {
        public City CityCoords { get; set; }
        public IEnumerable<int> Intervals { get; set; }
        public string OpenWeatherApiUrl { get; set; }
        public string OpenWeatherAppId { get; set; }
        public string OauthToken { get; set; }

    }

    public struct City
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
