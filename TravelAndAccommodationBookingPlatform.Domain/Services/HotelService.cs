using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
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
    private readonly IOwnerRepository _ownerRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public HotelService(IHotelRepository hotelRepository, IOwnerRepository ownerRepository,ICityRepository cityRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _ownerRepository = ownerRepository;
        _cityRepository = cityRepository;
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
    
    public async Task<PaginatedList<HotelDto>> GetAllHotelsAsync(int pageNumber, int pageSize)
    {
        var (hotels, totalCount) = await _hotelRepository.GetAllHotelsAsync(pageNumber, pageSize);
        var pageData = new PageData(totalCount, pageSize, pageNumber);
        var hotelDtos = _mapper.Map<IEnumerable<HotelDto>>(hotels);
        return new PaginatedList<HotelDto>(hotelDtos.ToList(), pageData);
    }
    
    public async Task<HotelDto> GetHotelByIdAsync(Guid hotelId)
    {
        var hotel = await _hotelRepository.GetHotelByIdAsync(hotelId);
        if (hotel == null)
        {
            throw new NotFoundException("Hotel not found.");
        }
        return _mapper.Map<HotelDto>(hotel);
    }
    
    public async Task CreateHotelAsync(CreateHotelDto hotelDto)
    {
        var owner = await _ownerRepository.GetOwnerByIdAsync(hotelDto.OwnerId);
        if (owner == null)
        {
            throw new NotFoundException("Owner not found.");
        }
        var city = await _cityRepository.GetCityByIdAsync(hotelDto.CityId);
        if (city == null)
        {
            throw new NotFoundException("City not found.");
        }
        var hotel = _mapper.Map<Hotel>(hotelDto);
        await _hotelRepository.CreateHotelAsync(hotel);
    }
    
    public async Task UpdateHotelAsync(Guid hotelId, UpdateHotelDto hotelDto)
    {
        var hotel = await _hotelRepository.GetHotelByIdAsync(hotelId);
        if (hotel == null)
        {
            throw new NotFoundException("Hotel not found.");
        }
        var owner = await _ownerRepository.GetOwnerByIdAsync(hotelDto.OwnerId);
        if (owner == null)
        {
            throw new NotFoundException("Owner not found.");
        }
        var city = await _cityRepository.GetCityByIdAsync(hotelDto.CityId);
        if (city == null)
        {
            throw new NotFoundException("City not found.");
        }
        _mapper.Map(hotelDto, hotel);
        await _hotelRepository.UpdateHotelAsync(hotel);
    }
    
    public async Task DeleteHotelAsync(Guid hotelId)
    {
        var hotel = await _hotelRepository.GetHotelByIdAsync(hotelId);
        if (hotel == null)
        {
            throw new NotFoundException("Hotel not found.");
        }
        await _hotelRepository.DeleteHotelAsync(hotelId);
    }
}