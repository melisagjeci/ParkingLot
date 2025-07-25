using Microsoft.AspNetCore.Mvc;
using ParkingLot.Models;

namespace ParkingLot.Controllers
{
    public class PricingPlansController : Controller
    {
        private readonly AppDbContext _context;

        public PricingPlansController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var plans = _context.PricingPlans.ToList();
            return View(plans);
        }
    }
}