using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface IBookingRepository
{
    Task<List<Hotel>> GetRecentlyVisitedHotelsAsync(Guid userId, int count);
}