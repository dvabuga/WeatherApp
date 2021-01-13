using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp.DB
{
    //модули, которые есть у сервиса
    public class ServiceModule
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ServiceID { get; set; }
        public Guid ModuleId { get; set; }
        public bool IsDefaultModule { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}