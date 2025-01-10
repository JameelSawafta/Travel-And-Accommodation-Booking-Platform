using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.Configurations;

public class HotelAmenityConfiguration : IEntityTypeConfiguration<HotelAmenity>
{
    public void Configure(EntityTypeBuilder<HotelAmenity> builder)
    {
        builder.HasKey(ha => ha.HotelAmenityId);
        builder.Property(ha => ha.HotelId).IsRequired();
        builder.Property(ha => ha.AmenityId).IsRequired();

        builder.HasOne(ha => ha.Hotel)
            .WithMany(h => h.HotelAmenities)
            .HasForeignKey(ha => ha.HotelId);

        builder.HasOne(ha => ha.Amenity)
            .WithMany(a => a.HotelAmenities)
            .HasForeignKey(ha => ha.AmenityId);
    }
}