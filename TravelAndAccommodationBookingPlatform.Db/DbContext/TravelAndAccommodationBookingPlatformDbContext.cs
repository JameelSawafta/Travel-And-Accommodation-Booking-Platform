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
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<RoomDiscount> RoomDiscounts { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<RoomAmenity> RoomAmenities { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Image> Images { get; set; }
    
    public TravelAndAccommodationBookingPlatformDbContext(DbContextOptions<TravelAndAccommodationBookingPlatformDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TravelAndAccommodationBookingPlatformDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}