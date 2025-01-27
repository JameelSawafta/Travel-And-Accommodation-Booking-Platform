using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.Configurations;

public class BookingDetailConfiguration : IEntityTypeConfiguration<BookingDetail>
{
    public void Configure(EntityTypeBuilder<BookingDetail> builder)
    {
        builder.HasKey(bd => bd.BookingDetailId);
        builder.Property(bd => bd.BookingId).IsRequired();
        builder.Property(bd => bd.RoomId).IsRequired();
        builder.Property(bd => bd.CheckInDate).IsRequired().HasColumnType("timestamp");
        builder.Property(bd => bd.CheckOutDate).IsRequired().HasColumnType("timestamp");
        builder.Property(bd => bd.Price).HasColumnType("decimal(18, 2)");

        builder.HasOne(bd => bd.Booking)
            .WithMany(b => b.BookingDetails)
            .HasForeignKey(bd => bd.BookingId);

        builder.HasOne(bd => bd.Room)
            .WithMany(r => r.BookingDetails)
            .HasForeignKey(bd => bd.RoomId);
    }
}