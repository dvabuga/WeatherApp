using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.DB;
using WeatherApp.Files;

namespace WeatherApp.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }


        [HttpPost]
        public void Upload(IFormFile uploadedFile)
        {
            var assm = GetAssembly(uploadedFile);
            //запись полученной сборки в бд

        }




        private (string AssemblyName, string AssemblyAuthor, byte[])? GetAssembly(IFormFile file)
        {
            (string, string, byte[])? assemblyInfo = null;

            using (var stream = file.OpenReadStream())
            {
                using (var archive = new ZipArchive(stream))
                {
                    var dlls = archive.Entries.Where(c => c.Name.IndexOf(".dll") != -1).ToList();

                    foreach (var dll in dlls) 
                    {
                        using (var archiveEntryStream = dll.Open())
                        {
                            using (var mem = new MemoryStream())
                            {
                                archiveEntryStream.CopyTo(mem);
                                var assemblyArr = mem.ToArray();
                                var assembly = Assembly.Load(assemblyArr.ToArray());
                                var hasAssemblyAttribute = assembly.GetCustomAttributes(typeof(ModuleAssemblyAttribute), true).Length > 0;

                                if (hasAssemblyAttribute)
                                {
                                    var assemblyAttribute = (ModuleAssemblyAttribute)assembly.GetCustomAttribute(typeof(ModuleAssemblyAttribute));
                                    var assemblyName = assemblyAttribute.name;
                                    var assemblyAuthor = assemblyAttribute.author;
                                    assemblyInfo = (assemblyName, assemblyAuthor, assemblyArr);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return assemblyInfo;
        }
    }
}