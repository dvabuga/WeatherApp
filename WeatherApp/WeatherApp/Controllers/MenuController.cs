using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WeatherApp.Authorization;

namespace WeatherApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public MenuController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        private List<(string, string, string)> menu = new List<(string, string, string)>
        {
            ("administrator", "I am admin", "Profile/AdminProfile"),
            ("developer","I am developer", "Profile/DeveloperProfile"),
            ("manager","I am manager", "Profile/ManagerProfile"),
            ("user","My profile", "Profile/UserProfile")
        };

        [HttpGet]
        public async Task<List<JObject>> GetProfileMenu()
        {
            var obj = new List<JObject>();
            var menuItems = new List<(string, string, string)>();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                var menuItem = menu.Where(c => c.Item1 == role).FirstOrDefault();
                var o = JObject.FromObject(new
                {
                    text = menuItem.Item2,
                    path = menuItem.Item3
                });
                obj.Add(o);
            }
            return obj;
        }
    }
}