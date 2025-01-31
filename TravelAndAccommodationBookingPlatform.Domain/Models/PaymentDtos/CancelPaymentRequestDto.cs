namespace TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

public class CancelPaymentRequestDto
{
    /// <summary>
    /// the payment id
    /// </summary>
    public Guid PaymentId { get; set; }
}