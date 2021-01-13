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
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceModule> ServiceModules { get; set; }
        public DbSet<UserServiceModule> UserServiceModules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Service>()
                .Property(e => e.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (ServiceType)Enum.Parse(typeof(ServiceType), v));
        }
    }
}
