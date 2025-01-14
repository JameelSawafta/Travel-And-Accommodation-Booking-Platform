using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.API.Validators.HomeValidators;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
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

    public HomeController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }
    
    [HttpGet("search")]
    public async Task<PaginatedList<HotelSearchResultDto>> SearchHotelsAsync([FromQuery] SearchRequestDto searchRequest, int pageSize, int pageNumber)
    {
       return await _hotelService.SearchHotelsAsync(searchRequest, pageSize, pageNumber);
    }
}