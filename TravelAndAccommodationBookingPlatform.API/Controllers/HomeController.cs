using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.API.Validators.HomeValidators;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.Common;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.SearchDtos;

namespace TravelAndAccommodationBookingPlatform.API.Controllers;

[ApiController]
[Route("api/home")]
[ApiVersion("1.0")]
public class HomeController : Controller
{
    private readonly IHotelService _hotelService;
    private readonly IBookingService _bookingService;
    private readonly ICityService _cityService;
    
    public HomeController(IHotelService hotelService, IBookingService bookingService, ICityService cityService)
    {
        _hotelService = hotelService;
        _bookingService = bookingService;
        _cityService = cityService;
    }

    [HttpGet("search")]
    public async Task<PaginatedList<HotelSearchResultDto>> SearchHotelsAsync([FromQuery] SearchRequestDto searchRequest,
        int pageSize, int pageNumber)
    {
        var validator = new SearchHotelsValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(searchRequest);

        return await _hotelService.SearchHotelsAsync(searchRequest, pageSize, pageNumber);
    }

    [HttpGet("featured-deals")]
    public async Task<List<FeaturedDealDto>> GetFeaturedDeals()
    {
        return await _hotelService.GetFeaturedDealsAsync(5);
    }

    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet("{userId}/recently-visited-hotels")]
    public async Task<List<RecentlyVisitedHotelDto>> GetRecentlyVisitedHotels(Guid userId)
    {
        return await _bookingService.GetRecentlyVisitedHotelsAsync(userId, 5);
    }

    [HttpGet("trending-destinations")]
    public async Task<List<TrendingDestinationDto>> GetTrendingDestinations()
    {
        return await _cityService.GetTrendingDestinationsAsync(5);
    }
}