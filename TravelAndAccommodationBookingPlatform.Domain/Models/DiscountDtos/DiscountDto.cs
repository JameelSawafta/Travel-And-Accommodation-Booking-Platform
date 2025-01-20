namespace TravelAndAccommodationBookingPlatform.Domain.Models.DiscountDtos;

public class DiscountDto
{
    public Guid DiscountId { get; set; } = Guid.NewGuid();
    public string? Description { get; set; }
    public double DiscountPercentageValue { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
}