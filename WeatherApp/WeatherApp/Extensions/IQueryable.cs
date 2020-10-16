
using System;
using System.Linq;
using WeatherApp.DB;

public static class IQueryable
{
    public static void AddOrUpdateByCityName(this IQueryable<Forecast> forecasts, ApplicationDbContext _context, string current, string daily)
    {
        var forecast = forecasts.Where(c => c.CityName == "Казань").FirstOrDefault();
        if (forecast == null)
        {
            forecast = new Forecast();
        }

        forecast.CityName = "Казань";
        forecast.CurrentForecast = current;
        forecast.DailyForecast = daily;


        if (forecast.Id == Guid.Empty)
        {
            forecast.Id = Guid.NewGuid();
            _context.Add(forecast);
        }
        else
        {
            _context.Update(forecast);
        }

        _context.SaveChanges();

    }
}