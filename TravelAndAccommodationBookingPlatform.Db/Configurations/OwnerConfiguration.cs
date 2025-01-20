using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Db.Configurations;

public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.HasKey(o => o.OwnerId);
        builder.Property(o => o.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(o => o.LastName).IsRequired().HasMaxLength(50);
        builder.Property(o => o.Email).IsRequired();
        builder.Property(o => o.PhoneNumber).HasMaxLength(20);
    }
}