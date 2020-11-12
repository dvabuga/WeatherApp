using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp.DB
{
    public class Module
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public byte[] Assembly { get; set; }
        public DateTimeOffset UploadDate { get; set; }

    }
}