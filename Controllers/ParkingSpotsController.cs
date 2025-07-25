using Microsoft.AspNetCore.Mvc;
using ParkingLot.Models;

namespace ParkingLot.Controllers
{
    public class ParkingSpotsController : Controller
    {
        private readonly AppDbContext _context;

        public ParkingSpotsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalSpots = _context.ParkingSpots.FirstOrDefault();

            int reservedSpots = _context.Subscriptions
                .Where(s => !s.IsDeleted && s.EndDate >= DateTime.Today)
                .Count();

            int freeSpots = totalSpots != null
                ? totalSpots.TotalSpots - reservedSpots
                : 0;

            ViewBag.ReservedSpots = reservedSpots;
            ViewBag.FreeSpots = freeSpots;

            return View(totalSpots);
        }
    }
}