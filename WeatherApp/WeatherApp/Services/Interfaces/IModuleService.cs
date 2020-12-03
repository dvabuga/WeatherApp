using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace WeatherApp.Services
{
    public interface IModuleService
    {
        //public byte[] GetAssembly(IFormFile file);
        public Type GetType(byte[] assembly);
        public object? InvokeMethod(Type t);
        public List<DB.Module> GetModules(Guid? moduleId = null, Guid? userId = null);
    }
}