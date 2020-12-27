using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp.DB
{
        public class UserCity
        {
            [Key]
            public Guid Id { get; set; }
            
            public Guid UserId { get; set; }

            public Guid CityId { get; set; }

            public DateTimeOffset SubscriptionDate { get; set; }

            public List<City> Cities { get; set; }
        }
}