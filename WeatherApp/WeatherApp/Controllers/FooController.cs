using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApp
{
    public class FooController : Controller
    {
        public IActionResult Index()
        {

            return Ok("1");
        }
    }
}