using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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


        private Dictionary<string, string> menu = new Dictionary<string, string>()
        {
            {"administrator", "Я-администратор"},
            {"developer","Я - разработчик"}
        };

        [HttpGet]
        public async Task<List<string>> GetProfileMenu()
        {
            var menuItems = new List<string>();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                var menuItem = menu.Where(c => c.Key == role).Select(c => c.Value).FirstOrDefault();
                menuItems.Add(menuItem);
            }
            return menuItems;
        }
    }
}