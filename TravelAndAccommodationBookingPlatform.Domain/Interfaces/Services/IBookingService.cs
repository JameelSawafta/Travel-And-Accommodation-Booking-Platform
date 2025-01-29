using TravelAndAccommodationBookingPlatform.Domain.Models;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

public interface IBookingService
{
    Task<List<RecentlyVisitedHotelDto>> GetRecentlyVisitedHotelsAsync(Guid userId, int count);
    Task<CheckoutDto> CreateBookingFromCartAsync(CheckoutRequestDto requestDto);
}