using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public BookingService(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }
    
    public async Task<List<RecentlyVisitedHotelDto>> GetRecentlyVisitedHotelsAsync(Guid userId, int count)
    {
        var hotels = await _bookingRepository.GetRecentlyVisitedHotelsAsync(userId, count);

        if (!hotels.Any())
        {
            throw new NotFoundException("No recently visited hotels found.");
        }

        return _mapper.Map<List<RecentlyVisitedHotelDto>>(hotels);
    }
}