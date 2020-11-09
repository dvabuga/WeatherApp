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
            
        }
    }
}