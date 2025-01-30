namespace TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

public class ConfirmPaymentRequestDto
{
    /// <summary>
    /// the payment id
    /// </summary>
    public Guid PaymentId { get; set; }
    /// <summary>
    /// the payer id
    /// </summary>
    public string PayerId { get; set; }
}