using ParkingLot.Models;

public class PriceCalculatorService
{
    private readonly AppDbContext _context;
    private const int GracePeriodMinutes = 15;

    public PriceCalculatorService(AppDbContext context)
    {
        _context = context;
    }

    public decimal CalculatePrice(Log log)
    {
        if (HasActiveSubscription(log))
            return 0;

        var duration = log.CheckOutTime - log.CheckInTime;

        if (IsWithinGracePeriod(duration))
            return 0;

        var pricingPlan = GetApplicablePricingPlan(log.CheckInTime);
        if (pricingPlan == null)
            throw new Exception("Pricing plan not found");

        return CalculateCharge(duration, pricingPlan);
    }

    private bool HasActiveSubscription(Log log) => log.Subscription != null;
    private bool IsWithinGracePeriod(TimeSpan duration)
    {
        return duration.TotalMinutes <= GracePeriodMinutes;
    }

    private PricingPlan GetApplicablePricingPlan(DateTime checkInTime)
    {
        var isWeekend = checkInTime.DayOfWeek == DayOfWeek.Saturday || checkInTime.DayOfWeek == DayOfWeek.Sunday;
        var type = isWeekend ? "E Shtunë - E Diel" : "E Hënë - E Premte";

        return _context.PricingPlans
            .FirstOrDefault(p => p.Type.ToLower() == type.ToLower());
    }

    private decimal CalculateCharge(TimeSpan duration, PricingPlan plan)
    {
        var totalHours = duration.TotalMinutes / 60.0;
        var fullDays = (int)(totalHours / 24);
        var remainingHours = totalHours % 24;

        decimal total = 0;

        if (totalHours <= plan.MinimumHours)
        {
            total = (decimal)totalHours * plan.HourlyRate;
        }
        else
        {
            total += fullDays * plan.DailyRate;

            if (remainingHours < plan.MinimumHours)
            {
                total += (decimal)remainingHours * plan.HourlyRate;
            }
            else
            {
                total += plan.DailyRate;
            }
        }

        return Math.Round(total, 2);
    }

    internal decimal CalculateManual(DateTime checkInTime, DateTime checkOutTime)
    {
        var duration = checkOutTime - checkInTime;
        var totalHours = (int)Math.Ceiling(duration.TotalHours);

        decimal pricePerHour = 1.00m; // cmimi x euro 1 euro pr or dmth
        decimal totalPrice = totalHours * pricePerHour;

        return totalPrice;
    }
}