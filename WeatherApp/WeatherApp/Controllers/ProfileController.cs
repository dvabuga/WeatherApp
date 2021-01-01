using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Services;


namespace WeatherApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IModuleService _moduleService;
        private readonly ICityService _cityService;

        public ProfileController(IModuleService moduleService, ICityService cityService)
        {
            _moduleService = moduleService;
            _cityService = cityService;
        }


        public IActionResult UserProfile()
        {
            var currentUserId = GetCurrentUserId();
            var userCities = _cityService.GetUserCities(currentUserId);


            return View(userCities);
        }

        public IActionResult AdminProfile()
        {
            return View();
        }

        public IActionResult DeveloperProfile()
        {
            var currentUserId = GetCurrentUserId();
            var modules = _moduleService.GetModules(null, currentUserId);
            //передавать модули во вьюху

            return View(modules);
        }

        public IActionResult ManagerProfile()
        {
            return View();
        }


        private Guid GetCurrentUserId()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Guid.Parse(currentUserId);
        }

    }
}