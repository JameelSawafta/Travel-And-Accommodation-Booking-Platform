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
    
}