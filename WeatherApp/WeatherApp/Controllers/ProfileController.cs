using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Services;


namespace WeatherApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IModuleService _moduleService;

        public ProfileController(IModuleService moduleService)
        {
            _moduleService = moduleService;
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
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            _moduleService.GetModules();
            //передавать модули во вьюху

            return View();
        }

        public IActionResult ManagerProfile()
        {
            return View();
        }
    }
}