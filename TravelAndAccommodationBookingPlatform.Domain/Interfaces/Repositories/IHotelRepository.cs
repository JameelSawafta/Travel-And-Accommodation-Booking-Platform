using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.SearchDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;

public interface IHotelRepository
{
    Task<(IEnumerable<Hotel>,int TotalCount)> SearchHotelsAsync(SearchRequestDto searchRequest, int pageSize, int pageNumber);
    Task<List<Hotel>> GetFeaturedDealsAsync(int count);
    Task<(IEnumerable<Hotel> Items, int TotalCount)> GetAllHotelsAsync(int pageNumber, int pageSize);
    Task<Hotel> GetHotelByIdAsync(Guid hotelId);
    Task CreateHotelAsync(Hotel hotel);
    Task UpdateHotelAsync(Hotel hotel);
    Task DeleteHotelAsync(Guid hotelId);
    Task<Hotel> GetHotelByIdWithRoomsAsync(Guid hotelId);
}