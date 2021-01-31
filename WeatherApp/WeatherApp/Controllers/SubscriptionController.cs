using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.DB;
using WeatherApp.Models;

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
            var services = _context.Services.Select(a => new ServiceSelection()
            {
                DateTimeCreate = a.DateTimeCreate,
                Description = a.Description,
                Id = a.Id,
                Name = a.Name,
                Type = a.Type,
                IsSelected = false
            }).ToList();
            var viewModel = new SubscriptionViewModel();

            viewModel.Services = services;

            return View(viewModel);
        }




    }
}