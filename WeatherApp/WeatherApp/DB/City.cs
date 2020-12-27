using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WeatherApp.DB 
{
        public class City
        {
            [Key]
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string Lat { get; set; }

            public string Lon { get; set; }

            public string Country { get; set; }
        }
}

