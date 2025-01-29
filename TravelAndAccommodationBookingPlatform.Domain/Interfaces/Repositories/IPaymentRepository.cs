using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task CreatePaymentAsync(Payment payment);
}