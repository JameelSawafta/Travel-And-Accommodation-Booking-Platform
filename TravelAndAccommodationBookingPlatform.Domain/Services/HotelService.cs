using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.Common;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.SearchDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Services;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public HotelService(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }
    
    
    
    public async Task<PaginatedList<HotelSearchResultDto>> SearchHotelsAsync(SearchRequestDto searchRequest, int pageSize, int pageNumber)
    {
        var (hotels, totalCount) = await _hotelRepository.SearchHotelsAsync(searchRequest, pageSize, pageNumber);

        if (totalCount == 0)
        {
            throw new NotFoundException("No hotels found");
        }
        
        if (pageNumber > Math.Ceiling((double)totalCount / pageSize))
        {
            throw new NotFoundException("Invalid page number");
        }
        
        var pageData = new PageData(totalCount, pageSize, pageNumber);
        var result = _mapper.Map<IEnumerable<HotelSearchResultDto>>(hotels).ToList();

        return new PaginatedList<HotelSearchResultDto>(result, pageData);
    }
    
    
    public async Task<List<FeaturedDealDto>> GetFeaturedDealsAsync(int count)
    {
        var hotels = await _hotelRepository.GetFeaturedDealsAsync(count);

        if (!hotels.Any())
        {
            throw new NotFoundException("No featured deals found");
        }

        var now = DateTime.Now;

        var featuredDeals = hotels.Select(hotel =>
            {
                var validRooms = hotel.Rooms
                    .Where(r => r.RoomDiscounts.Any(rd => rd.Discount.ValidFrom <= now && rd.Discount.ValidTo >= now));

                var roomWithMaxDiscount = validRooms
                    .Select(r => new
                    {
                        Room = r,
                        MaxDiscount = r.RoomDiscounts
                            .Where(rd => rd.Discount.ValidFrom <= now && rd.Discount.ValidTo >= now)
                            .Max(rd => rd.Discount.DiscountPercentageValue)
                    })
                    .OrderByDescending(r => r.MaxDiscount)
                    .FirstOrDefault();

                if (roomWithMaxDiscount == null)
                {
                    return null;
                }

                var dto = _mapper.Map<FeaturedDealDto>(hotel);
                dto.OriginalPrice = roomWithMaxDiscount.Room.PricePerNight;
                dto.DiscountedPrice = roomWithMaxDiscount.Room.PricePerNight * (1 - (decimal)roomWithMaxDiscount.MaxDiscount);
                return dto;
            })
            .Where(dto => dto != null) 
            .ToList();

        return featuredDeals;
    }

}