using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.Configurations;

public class RoomDiscountConfiguration : IEntityTypeConfiguration<RoomDiscount>
{
    public void Configure(EntityTypeBuilder<RoomDiscount> builder)
    {
        builder.HasKey(rd => rd.RoomDiscountId);
        builder.Property(rd => rd.RoomId).IsRequired();
        builder.Property(rd => rd.DiscountId).IsRequired();

        builder.HasOne(rd => rd.Room)
            .WithMany(r => r.RoomDiscounts)
            .HasForeignKey(rd => rd.RoomId);

        builder.HasOne(rd => rd.Discount)
            .WithMany(d => d.RoomDiscounts)
            .HasForeignKey(rd => rd.DiscountId);
    }
}