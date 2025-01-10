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
        builder.Property(d => d.DiscountType).HasConversion<int>().IsRequired();
        builder.Property(d => d.DiscountValue).IsRequired();
        builder.Property(d => d.ValidFrom).IsRequired();
        builder.Property(d => d.ValidTo).IsRequired();
    }
}