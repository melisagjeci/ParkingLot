using Microsoft.AspNetCore.Mvc;
using ParkingLot.Models;

namespace ParkingLot.Controllers   
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricingPlanController : ControllerBase     //vtm per get dhe update plan ky controllr 
    {
        private readonly AppDbContext _context;

        public PricingPlanController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPlans() => Ok(_context.PricingPlans.ToList());

        [HttpPut("{id}")]
        public IActionResult UpdatePlan(int id, [FromBody] PricingPlan updated)
        {
            var plan = _context.PricingPlans.Find(id);
            if (plan == null) return NotFound();

            plan.HourlyRate = updated.HourlyRate;
            plan.DailyRate = updated.DailyRate;
            plan.MinimumHours = updated.MinimumHours;
            plan.Type = updated.Type;

            _context.SaveChanges();
            return Ok(plan);
        }
    }
}
