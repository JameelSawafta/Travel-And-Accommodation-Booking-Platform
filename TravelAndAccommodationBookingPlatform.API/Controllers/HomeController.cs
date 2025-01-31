using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.API.Validators.ModelsValidators.HomeValidators;
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

    /// <summary>
    /// Searches for hotels based on the provided search criteria.
    /// </summary>
    /// <param name="searchRequest">The search criteria including query, dates, and room requirements.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <returns>A paginated list of hotels matching the search criteria.</returns>
    /// <response code="200">Returns the paginated list of hotels.</response>
    /// <response code="400">If the search request is invalid.</response>
    /// <response code="404">If no hotels match the search criteria.</response>
    [HttpGet("search")]
    public async Task<PaginatedList<HotelSearchResultDto>> SearchHotelsAsync([FromQuery] SearchRequestDto searchRequest,
        int pageSize, int pageNumber)
    {
        var validator = new SearchHotelsValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(searchRequest);

        return await _hotelService.SearchHotelsAsync(searchRequest, pageSize, pageNumber);
    }

    /// <summary>
    /// Retrieves a list of featured hotel deals.
    /// </summary>
    /// <returns>A list of featured hotel deals with discounted prices.</returns>
    /// <response code="200">Returns the list of featured deals.</response>
    /// <response code="404">If no featured deals are found.</response>
    [HttpGet("featured-deals")]
    public async Task<List<FeaturedDealDto>> GetFeaturedDeals()
    {
        return await _hotelService.GetFeaturedDealsAsync(5);
    }

    /// <summary>
    /// Retrieves a list of recently visited hotels for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of recently visited hotels by the user.</returns>
    /// <response code="200">Returns the list of recently visited hotels.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized to access this resource.</response>
    /// <response code="404">If no recently visited hotels are found for the user.</response>
    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet("{userId}/recently-visited-hotels")]
    public async Task<List<RecentlyVisitedHotelDto>> GetRecentlyVisitedHotels(Guid userId)
    {
        return await _bookingService.GetRecentlyVisitedHotelsAsync(userId, 5);
    }

    /// <summary>
    /// Retrieves a list of trending destinations based on booking activity.
    /// </summary>
    /// <returns>A list of trending destinations with their details.</returns>
    /// <response code="200">Returns the list of trending destinations.</response>
    /// <response code="404">If no trending destinations are found.</response>
    [HttpGet("trending-destinations")]
    public async Task<List<TrendingDestinationDto>> GetTrendingDestinations()
    {
        return await _cityService.GetTrendingDestinationsAsync(5);
    }
}