using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.Controllers
{
    public class ProfileController : Controller
    {

        public ProfileController()
        {

        }


        public IActionResult UserProfile()
        {
            return View();
        }

        public IActionResult AdminProfile()
        {
            return View();
        }

        public IActionResult DeveloperProfile()
        {
            return View();
        }

        public IActionResult ManagerProfile()
        {
            return View();
        }
    }
}