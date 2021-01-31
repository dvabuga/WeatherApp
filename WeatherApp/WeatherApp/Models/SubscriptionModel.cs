using System;
using System.Collections.Generic;
using WeatherApp.DB;

namespace WeatherApp.Models
{
    //возможная модель для формы подписки на сервисы

    public class ServiceSelection
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ServiceType Type { get; set; } //free, paid
        public string Description { get; set; }
        public DateTimeOffset DateTimeCreate { get; set; }
        public bool IsSelected { get; set; }

    }


    public class SubscriptionViewModel
    {
        public City City { get; set; }
        public List<ServiceSelection> Services { get; set; }
    }
}