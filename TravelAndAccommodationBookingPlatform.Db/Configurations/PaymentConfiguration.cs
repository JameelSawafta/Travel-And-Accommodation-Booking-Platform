using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.PaymentId);
        builder.Property(p => p.BookingId).IsRequired();
        builder.Property(p => p.Amount).HasColumnType("decimal(18, 2)");
        builder.Property(p => p.PaymentMethod).HasConversion<int>().IsRequired();
        builder.Property(p => p.TransactionID).HasMaxLength(50);
        builder.Property(p => p.TransactionDate).IsRequired();
        builder.Property(p => p.Status).HasConversion<int>().IsRequired();

        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BookingId);
    }
}