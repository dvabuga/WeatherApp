using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp.DB
{
    //модули для сервиса, который выбрал пользователь
    public class UserServiceModule
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid ModuleId { get; set; }
    }
}