using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.BookingId);
        builder.Property(b => b.UserId).IsRequired();
        builder.Property(b => b.RoomId).IsRequired();
        builder.Property(b => b.CheckInDate).IsRequired();
        builder.Property(b => b.CheckOutDate).IsRequired();
        builder.Property(b => b.TotalPrice).HasColumnType("decimal(18, 2)");
        builder.Property(b => b.Status).HasConversion<int>().HasMaxLength(50);

        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId);

        builder.HasOne(b => b.Room)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RoomId);
        
        builder.HasIndex(b => b.CheckInDate);

        builder.HasIndex(b => b.CheckOutDate);
    }
}