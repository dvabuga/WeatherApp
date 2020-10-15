
using System;
using System.Collections.Generic;

namespace WeatherApp.Services.Interfaces
{
    public interface IDisplayChart
    {
        public List<DateTimeOffset> GetIntervalTimePoints(int intervalStep, DateTimeOffset date1, DateTimeOffset? date2 = null);

    }
}