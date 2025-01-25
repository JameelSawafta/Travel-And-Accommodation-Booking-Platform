using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface IOwnerRepository
{
    Task<Owner> GetOwnerByIdAsync(Guid ownerId);
}