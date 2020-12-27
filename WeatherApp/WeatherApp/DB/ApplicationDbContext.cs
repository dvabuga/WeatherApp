using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherApp.DB;


namespace WeatherApp.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Forecast> Forecasts { get; set; }

        public DbSet<Module> Modules { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<UserCity> UserCities { get; set; }
    }
}
