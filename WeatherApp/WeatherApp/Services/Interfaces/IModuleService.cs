using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace WeatherApp.Services
{
    public interface IModuleInterface
    {
        //public byte[] GetAssembly(IFormFile file);
        public Type GetType(byte[] assembly);
        public object? InvokeMethod(Type t);
    }
}