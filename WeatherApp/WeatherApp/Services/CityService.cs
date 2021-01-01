using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WeatherApp.DB;

namespace WeatherApp.Services
{
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext _context;

        public CityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UserCity> GetUserCities(Guid userId)
        {
            var userCities = _context.UserCities
                                     .Include(c => c.City)
                                     .Where(c => c.UserId == userId)
                                     .ToList();

            return userCities;
        }
    }

}