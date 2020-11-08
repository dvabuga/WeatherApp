using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Files;

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
                    var dlls = entities.Where(c => c.Name.IndexOf(".dll") != -1).ToList();

                    foreach (var dll in dlls)
                    {
                        using (var ss = dll.Open())
                        {
                            using (var mem = new MemoryStream())
                            {
                                ss.CopyTo(mem);
                                var cont = Assembly.Load(mem.ToArray());
                                var t = cont.GetTypes().Where(t => t.GetCustomAttributes(typeof(ModuleAttribute), true).Length > 0).FirstOrDefault();
                                if (t != null)
                                {
                                    var inst = Activator.CreateInstance(t);
                                    var r = t.InvokeMember("Get", BindingFlags.InvokeMethod, null, inst, null);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}