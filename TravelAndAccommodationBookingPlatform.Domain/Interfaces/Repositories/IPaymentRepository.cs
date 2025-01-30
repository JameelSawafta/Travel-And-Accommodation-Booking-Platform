using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task CreatePaymentAsync(Payment payment);
    Task<Payment?> GetSuccessPaymentWithBookingDetailsByIdAsync(Guid paymentId);
    Task<Payment?> GetPaymentWithBookingByIdAsync(Guid paymentId);
    Task UpdatePaymentAsync(Payment payment);
}