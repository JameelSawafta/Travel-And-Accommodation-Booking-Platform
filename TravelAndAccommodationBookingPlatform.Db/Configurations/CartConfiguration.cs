using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.CartId);
        builder.Property(c => c.UserId).IsRequired();
        builder.Property(c => c.RoomId).IsRequired();
        builder.Property(c => c.CheckInDate).IsRequired().HasColumnType("timestamp");
        builder.Property(c => c.CheckOutDate).IsRequired().HasColumnType("timestamp");
        builder.Property(c => c.Price).IsRequired().HasColumnType("decimal(18,2)");

        builder.HasOne(c => c.Room)
            .WithMany(r => r.Carts)
            .HasForeignKey(c => c.RoomId);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Carts)
            .HasForeignKey(c => c.UserId);
    }
}