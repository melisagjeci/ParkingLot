using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class SubscriptionsController : Controller
{
    private readonly AppDbContext _context;

    public SubscriptionsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var subs = _context.Subscriptions
            .Include(s => s.Subscriber)
            .Include(s => s.PricingPlan)
            .Where(s => !s.IsDeleted)
            .ToList();

        return View(subs);
    }
}