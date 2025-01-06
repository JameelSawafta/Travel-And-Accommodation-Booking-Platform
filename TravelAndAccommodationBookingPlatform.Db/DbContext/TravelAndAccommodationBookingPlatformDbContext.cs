using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.DbContext;

public class TravelAndAccommodationBookingPlatformDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingDetail> BookingDetails { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    
    public TravelAndAccommodationBookingPlatformDbContext(DbContextOptions<TravelAndAccommodationBookingPlatformDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        base.OnModelCreating(modelBuilder);
    }
}