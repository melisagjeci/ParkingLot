using Microsoft.AspNetCore.Mvc;

public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var totalSpots = _context.ParkingSpots.Count();

        var activeSubscriptions = _context.Subscriptions
            .Where(s => !s.IsDeleted && s.EndDate >= DateTime.Today)
            .Count();

        var reservedSpots = activeSubscriptions;
        var regularSpots = totalSpots - reservedSpots;

        var occupiedLogs = _context.Logs
            .Where(l => l.CheckInTime != null && l.CheckOutTime == null)
            .Count();

        var regularOccupied = occupiedLogs; 
        var regularFree = regularSpots - regularOccupied;
        var reservedFree = reservedSpots; 

        var dashboard = new
        {
            TotalSpots = totalSpots,
            ReservedSpots = reservedSpots,
            RegularSpots = regularSpots,
            RegularFreeSpots = regularFree,
            RegularOccupiedSpots = regularOccupied,
            ReservedFreeSpots = reservedFree
        };

        return View(dashboard);
    }
}