using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherApp.Authorization;
using WeatherApp.DB;
using WeatherApp.Files.SharedModels;
using WeatherApp.Services;
using WeatherApp.Services.Interfaces;


namespace WeatherApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["DefaultConnection"];
            var migrationsAssembly = typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<ApplicationDbContext>(options =>
                                                options.UseNpgsql(connectionString, b =>
                                                            b.MigrationsAssembly(migrationsAssembly)));

            var authConnectionString = Configuration["AuthConnection"];
            var authMigrationsAssembly = typeof(IdentityContext).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<IdentityContext>(options =>
                                                options.UseNpgsql(authConnectionString, b =>
                                                            b.MigrationsAssembly(authMigrationsAssembly)));


            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<IdentityContext>();

            // services.AddHostedService<UploadService>();

            services.AddTransient<IStorageService, YandexStorage>();
            services.AddTransient<IGetForecast, ForecastBase>();
            services.AddTransient<IForecastService, OpenWeatherService>();
            services.AddTransient<IFaultService, FaultService>();
            services.AddHttpClient();

            services.AddOptions();
            var section = Configuration.GetSection("WeatherServiceSettings");
            services.Configure<WeatherServiceSettings>(section);

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{Id?}");
            });
        }
    }
}
