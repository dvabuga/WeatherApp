using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using WeatherApp.Files;

namespace WeatherApp.Services
{
    public class ModuleService : IModuleInterface
    {
        
        public Type GetType(byte[] assembly)
        {
            var a = Assembly.Load(assembly.ToArray());
            var type = a.GetTypes()
                        .Where(t => t.GetCustomAttributes(typeof(ModuleAttribute), true).Length > 0)
                        .FirstOrDefault();

            return type;
        }


        public object InvokeMethod(Type t)
        {
            var method = t.GetMethods()
                        .Where(m => m.GetCustomAttributes(typeof(GetDataAttribute), true).Length > 0)
                        .FirstOrDefault().Name;

            var instance = Activator.CreateInstance(t);
            var result = t.InvokeMember(method, BindingFlags.InvokeMethod, null, instance, null);

            return result;
        }
    }
}