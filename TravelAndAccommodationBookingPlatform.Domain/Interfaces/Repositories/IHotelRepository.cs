using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.SearchDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface IHotelRepository
{
    Task<(IEnumerable<Hotel>,int TotalCount)> SearchHotelsAsync(SearchRequestDto searchRequest, int pageSize, int pageNumber);
}