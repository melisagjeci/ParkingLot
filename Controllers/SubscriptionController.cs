using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingLot.Models;

namespace ParkingLot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubscriptionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateSubscription([FromBody] Subscription subscription)
        {
            if (_context.Subscriptions.Any(s => s.Code == subscription.Code))
                return BadRequest("Subscription code already exists");

            var days = (subscription.EndDate - subscription.StartDate).Days;
            var plan = _context.PricingPlans.FirstOrDefault(p => p.Type.ToLower() == "");
            if (plan == null) return BadRequest("Weekday pricing plan not found");

            var basePrice = days * plan.DailyRate;
            subscription.Price = basePrice - subscription.DiscountValue;

            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();
            return Ok(subscription);
        }

        [HttpGet]
        public IActionResult GetSubscriptions(string? search)
        {
            var query = _context.Subscriptions.Include(s => s.Subscriber).Where(s => !s.IsDeleted);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    s.Code.Contains(search) ||
                    s.Subscriber.FirstName.Contains(search) ||
                    s.Subscriber.LastName.Contains(search));
            }

            return Ok(query.ToList());
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSubscription(int id, [FromBody] Subscription updated)
        {
            var subscription = _context.Subscriptions.Find(id);
            if (subscription == null || subscription.IsDeleted) return NotFound();

            subscription.StartDate = updated.StartDate;
            subscription.EndDate = updated.EndDate;
            subscription.DiscountValue = updated.DiscountValue;
            subscription.Price = updated.Price;

            _context.SaveChanges();
            return Ok(subscription);
        }

        [HttpDelete("{id}")]
        public IActionResult SoftDeleteSubscription(int id)
        {
            var sub = _context.Subscriptions.Find(id);
            if (sub == null) return NotFound();

            sub.IsDeleted = true;
            _context.SaveChanges();
            return NoContent();
        }
        [HttpGet("manage")]
        public IActionResult GetManageableSubscriptions()
        {
            var subscriptions = _context.Subscriptions
                .Include(s => s.Subscriber)
                .Include(s => s.PricingPlan)
                .Where(s => !s.IsDeleted)
                .Select(s => new
                {
                    s.Id,
                    s.Code,
                    SubscriberName = s.Subscriber.FirstName + " " + s.Subscriber.LastName,
                    s.Subscriber.Email,
                    s.Subscriber.PlateNumber,
                    PricingPlan = s.PricingPlan.Type,
                    s.StartDate,
                    s.EndDate,
                    s.Price,
                    Status = DateTime.Now <= s.EndDate ? "Aktive" : "Skaduar"
                })
                .ToList();

            return Ok(subscriptions);
        }
    }
}
