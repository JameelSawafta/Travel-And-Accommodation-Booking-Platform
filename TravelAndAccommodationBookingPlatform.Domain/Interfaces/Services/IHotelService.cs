using TravelAndAccommodationBookingPlatform.Domain.Models.Common;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.SearchDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface IHotelService
{
    Task<PaginatedList<HotelSearchResultDto>> SearchHotelsAsync(SearchRequestDto searchRequest, int pageSize, int pageNumber);
    Task<List<FeaturedDealDto>> GetFeaturedDealsAsync(int count);
    Task<PaginatedList<HotelDto>> GetAllHotelsAsync(int pageNumber, int pageSize);
    Task<HotelDto> GetHotelByIdAsync(Guid hotelId);
    Task CreateHotelAsync(CreateHotelDto hotelDto);
    Task UpdateHotelAsync(Guid hotelId, UpdateHotelDto hotelDto);
    Task DeleteHotelAsync(Guid hotelId);
    Task<HotelDetailedDto> GetHotelByIdWithRoomsAsync(Guid hotelId);
}