using TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface IPaymentService
{
    Task ConfirmPaymentAsync(ConfirmPaymentRequestDto request);
    Task CancelPaymentAsync(CancelPaymentRequestDto request);
    Task<byte[]> GeneratePaymentPdfAsync(Guid paymentId);
}