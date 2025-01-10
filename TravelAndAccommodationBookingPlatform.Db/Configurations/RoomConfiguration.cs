using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.Configurations;


public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.RoomId);
        builder.Property(r => r.HotelId).IsRequired();
        builder.Property(r => r.RoomNumber).IsRequired().HasMaxLength(10);
        builder.Property(r => r.PricePerNight).HasColumnType("decimal(18, 2)").IsRequired();
        builder.Property(r => r.RoomType).HasConversion<int>().IsRequired();
        builder.Property(r => r.Description).HasMaxLength(500);
        builder.Property(r => r.AdultsCapacity).IsRequired();
        builder.Property(r => r.ChildrenCapacity).IsRequired();
        builder.Property(r => r.Availability).IsRequired();

        builder.HasOne(r => r.Hotel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HotelId);
        
        builder.HasIndex(r => r.RoomNumber);

        builder.HasIndex(r => new { r.HotelId, r.RoomNumber }).IsUnique();
    }
}