using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(c => c.CityId);
        builder.Property(c => c.CityName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Country).IsRequired().HasMaxLength(100);
        builder.Property(c => c.PostCode).HasMaxLength(20);
        builder.Property(c => c.ThumbnailUrl).HasMaxLength(200);
        
        builder.HasIndex(c => c.CityName).IsUnique();
    }
}