using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {


            return View();
        }
    }
}