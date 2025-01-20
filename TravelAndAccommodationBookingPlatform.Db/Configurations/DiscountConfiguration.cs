using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.Configurations;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(d => d.DiscountId);
        builder.Property(d => d.Description).HasMaxLength(500);
        builder.Property(d => d.DiscountPercentageValue).IsRequired().HasColumnType("decimal(3,2)");
        builder.Property(d => d.ValidFrom).IsRequired().HasColumnType("timestamp");;
        builder.Property(d => d.ValidTo).IsRequired().HasColumnType("timestamp");;
    }
}