using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp.DB
{
    //сервисы - прогноз погоды, поиск кафе и т.д
    public class Service
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ServiceType Type { get; set; } //free, paid
        public string Description { get; set; }
        public DateTimeOffset DateTimeCreate { get; set; }
    }

}