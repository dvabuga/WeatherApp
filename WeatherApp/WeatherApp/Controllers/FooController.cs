using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.Controllers
{
    public class FooController : Controller
    {
        public IActionResult Index()
        {

            return Ok("1");
        }


        public IActionResult IndexTwo()
        {

            return Ok("2");
        }
    }
}