using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.DB;
using WeatherApp.Files;
using Module = WeatherApp.DB.Module;

namespace WeatherApp.Controllers
{
    
    [Authorize(Roles="developer")]
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


        public IActionResult GetModules([FromQuery]Guid? Id = null)
        {
            var modules = new List<Module>();
            var modulesQuery = _context.Modules.AsQueryable();

            if (Id == null)
            {
                modules = modulesQuery.ToList();
            }
            else
            {
                modules = modulesQuery.Where(c => c.Id == Id).ToList();
            }


            return View(modules);
            // return Ok(new
            // {
            //     result = "success",
            //     value = modules
            // });

        }

        
        public IActionResult Add(IFormFile uploadedFile)
        {
            var module = GetModule(uploadedFile);
            var moduleAlredyExsit = _context.Modules.Where(c => c.Name == module.Value.ModuleName &
                                   c.Author == module.Value.ModuleAuthor &
                                   c.Version == module.Value.ModuleVersion).Any();

            if (moduleAlredyExsit)
            {
                return Ok(new
                {
                    result = "error",
                    message = "module with such propertys alredy exists"
                });
            }

            var newModule = new Module()
            {
                Id = Guid.NewGuid(),
                Author = module.Value.ModuleAuthor,
                Name = module.Value.ModuleName,
                Version = module.Value.ModuleVersion,
                Assembly = module.Value.Item4,
                UploadDate = DateTimeOffset.Now
            };
            _context.Add(newModule);
            _context.SaveChanges();

            return RedirectToAction("Get");
            // return Ok(new
            // {
            //     result = "success",
            //     value = newModule.Id
            // });
        }



        private (string ModuleName, string ModuleAuthor, string ModuleVersion, byte[])? GetModule(IFormFile file)
        {
            (string, string, string, byte[])? moduleInfo = null;

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
                                    var moduleName = assemblyAttribute.name;
                                    var moduleAuthor = assemblyAttribute.author;
                                    var moduleVersion = assemblyAttribute.version;
                                    moduleInfo = (moduleName, moduleAuthor, moduleVersion, assemblyArr);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return moduleInfo;
        }
    }
}