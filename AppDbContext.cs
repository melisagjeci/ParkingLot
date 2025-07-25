using Microsoft.EntityFrameworkCore;
using ParkingLot.Models;

public class AppDbContext : DbContext
{
    public DbSet<ParkingSpot> ParkingSpots { get; set; }
    public DbSet<PricingPlan> PricingPlans { get; set; }
    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Log> Logs { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscriber>()
            .HasIndex(s => s.IdCardNumber)
            .IsUnique();

        modelBuilder.Entity<Subscription>()
            .HasIndex(s => s.Code)
            .IsUnique();

        modelBuilder.Entity<Log>()
            .HasIndex(l => l.Code)
            .IsUnique();

        modelBuilder.Entity<Subscription>()
            .Property(s => s.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Subscription>()
            .Property(s => s.DiscountValue)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Log>()
            .Property(l => l.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<PricingPlan>()
            .Property(p => p.DailyRate)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<PricingPlan>()
            .Property(p => p.HourlyRate)
            .HasColumnType("decimal(18,2)");
    }
}