using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WeatherApp.DB;

namespace WeatherApp.Services
{
    public interface ICityService
    {
        public List<UserCity> GetUserCities(Guid userId);





    }
}