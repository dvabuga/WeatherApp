using Microsoft.AspNetCore.Mvc;
using WeatherApp.DB;

namespace WeatherApp.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AddSubscription()
        {

            return View();
        }




    }
}