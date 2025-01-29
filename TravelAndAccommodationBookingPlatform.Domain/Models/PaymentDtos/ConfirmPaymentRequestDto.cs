namespace TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

public class ConfirmPaymentRequestDto
{
    /// <summary>
    /// the booking id
    /// </summary>
    public Guid BookingId { get; set; }
    /// <summary>
    /// the payment id
    /// </summary>
    public string PaymentId { get; set; }
    /// <summary>
    /// the payer id
    /// </summary>
    public string PayerId { get; set; }
}