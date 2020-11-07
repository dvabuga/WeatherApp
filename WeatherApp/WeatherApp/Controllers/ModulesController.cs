using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.Controllers
{
    public class ModulesController : Controller
    {

        public IActionResult Index()
        {

            return View();
        }


        [HttpPost]
        public void Upload(IFormFile uploadedFile)
        {
            using (var stream = uploadedFile.OpenReadStream())
            {
                using (var archive = new ZipArchive(stream))
                {
                    var entities = archive.Entries;
                    var dll = entities.Where(c => c.Name.Contains(".dll")).FirstOrDefault();

                    using (var ss = dll.Open())
                    {
                        using (var mem = new MemoryStream())
                        {
                            ss.CopyTo(mem);
                            var cont = Assembly.Load(mem.ToArray());
                            var all = cont.GetTypes();
                            var t = cont.GetType("WeatherApp.WeatherModule.Controllers.WeatherForecastController");
                            var inst = Activator.CreateInstance(t);
                            var r = t.InvokeMember("Get",  BindingFlags.InvokeMethod, null, inst, null);
                        }
                    }
                }
            }
        }
    }
}