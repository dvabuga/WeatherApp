using System;
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
            var modules = _moduleService.GetModules(null, Guid.Parse(currentUserId));
            //передавать модули во вьюху

            return View(modules);
        }

        public IActionResult ManagerProfile()
        {
            return View();
        }
    }
}