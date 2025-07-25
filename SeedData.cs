using ParkingLot.Models;

namespace ParkingLot
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (!context.ParkingSpots.Any())
            {
                context.ParkingSpots.Add(new ParkingSpot { TotalSpots = 100 });
            }

            if (!context.PricingPlans.Any())
            {
                context.PricingPlans.AddRange(
                    new PricingPlan { HourlyRate = 1, DailyRate = 5, MinimumHours = 3, Type = "E Hënë - E Premte" },
                    new PricingPlan { HourlyRate = 1.5m, DailyRate = 6, MinimumHours = 2, Type = "E Shtunë - E Diel" }
                );
            }

            context.SaveChanges();
        }
    }
}
