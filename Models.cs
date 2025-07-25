using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ParkingLot.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }
        public int TotalSpots { get; set; }
    }

    public class PricingPlan
    {
        public int Id { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal DailyRate { get; set; }
        public int MinimumHours { get; set; }
        public string Type { get; set; } 
    }

    public class Subscriber
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [MaxLength(20)]
        public string IdCardNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public DateTime Birthday { get; set; }

        [Required]
        public string PlateNumber { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }

    public class Subscription
    {
        public int Id { get; set; }

        public int SubscriberId { get; set; }
        public Subscriber Subscriber { get; set; }

        public int PricingPlanId { get; set; }
        public PricingPlan PricingPlan { get; set; }


        public string Code { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountValue { get; set; }
        public bool IsDeleted { get; set; }
        public string PlateNumber { get; internal set; }
    }

    public class Log
    {
        internal string? Code;

        public string? PlateNumber { get; internal set; }
        public DateTime CheckInTime { get; internal set; }
        public Subscription? Subscription { get; internal set; }
        public DateTime CheckOutTime { get; internal set; }
        public decimal Price { get; set; } 
        public int Id { get; internal set; }
        public int SubscriptionId { get; internal set; }

        

    }

}