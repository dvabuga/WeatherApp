using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using WeatherApp.DB;
using WeatherApp.Files;
using Module = WeatherApp.DB.Module;

namespace WeatherApp.Services
{
    public class ModuleService : IModuleService
    {
        private readonly ApplicationDbContext _context;

        public ModuleService(ApplicationDbContext context)
        {
            _context = context;
        }
        
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


        public List<Module> GetModules(Guid? moduleId, Guid? userId )
        {

            var modules = new List<Module>();
            var modulesQuery = _context.Modules.AsQueryable();

            if(userId != null)
            {
                modulesQuery = modulesQuery.Where(c=>c.CreatedUserId == userId);
            }

            if (moduleId == null)
            {
                modules = modulesQuery.ToList();
            }
            else
            {
                modules = modulesQuery.Where(c => c.Id == moduleId).ToList();
            }

            return modules;
        }
    }
}