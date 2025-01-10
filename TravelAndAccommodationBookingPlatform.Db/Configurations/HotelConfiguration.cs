using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasKey(h => h.HotelId);
        builder.Property(h => h.HotelName).IsRequired().HasMaxLength(100);
        builder.Property(h => h.Description).HasMaxLength(500);
        builder.Property(h => h.StarRating).IsRequired();
        builder.Property(h => h.OwnerId).IsRequired();
        builder.Property(h => h.PhoneNumber).IsRequired();
        builder.Property(h => h.Address).IsRequired().HasMaxLength(200);

        builder.HasOne(h => h.City)
            .WithMany(c => c.Hotels)
            .HasForeignKey(h => h.CityId);

        builder.HasOne(h => h.Owner)
            .WithMany(o => o.Hotels)
            .HasForeignKey(h => h.OwnerId);
        
        builder.HasIndex(h => h.HotelName);
        
        builder.HasIndex(h => new { h.CityId, h.HotelName });
    }
}