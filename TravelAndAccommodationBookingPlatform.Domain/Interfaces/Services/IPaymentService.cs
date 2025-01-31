using TravelAndAccommodationBookingPlatform.Domain.Models.PaymentDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface IPaymentService
{
    Task<PaymentDto> GetPaymentWithBookingDetailsByIdAsync(Guid paymentId);
    Task<PaymentResponsetDto> ConfirmPaymentAsync(ConfirmPaymentRequestDto request);
    Task CancelPaymentAsync(CancelPaymentRequestDto request);
    Task<byte[]> GeneratePaymentInvoiceAsync(Guid paymentId);
}