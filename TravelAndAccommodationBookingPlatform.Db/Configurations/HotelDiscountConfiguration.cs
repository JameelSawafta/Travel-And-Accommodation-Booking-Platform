using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.Configurations;

public class HotelDiscountConfiguration : IEntityTypeConfiguration<HotelDiscount>
{
    public void Configure(EntityTypeBuilder<HotelDiscount> builder)
    {
        builder.HasKey(hd => hd.HotelDiscountId);
        builder.Property(hd => hd.HotelId).IsRequired();
        builder.Property(hd => hd.DiscountId).IsRequired();

        builder.HasOne(hd => hd.Hotel)
            .WithMany(h => h.HotelDiscounts)
            .HasForeignKey(hd => hd.HotelId);

        builder.HasOne(hd => hd.Discount)
            .WithMany(d => d.HotelDiscounts)
            .HasForeignKey(hd => hd.DiscountId);
    }
}