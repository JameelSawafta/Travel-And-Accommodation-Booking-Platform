using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.Common;

namespace TravelAndAccommodationBookingPlatform.API.Controllers;

[ApiController]
[Route("api/cities")]
[ApiVersion("1.0")]
public class CitiesController : Controller
{
    private readonly ICityService _cityService;
    
    public CitiesController(ICityService cityService)
    {
        _cityService = cityService;
    }
    
    /// <summary>
    /// Retrieves a paginated list of all cities.
    /// </summary>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <returns>A paginated list of cities.</returns>
    /// <response code="200">Returns the paginated list of cities.</response>
    [HttpGet]
    public async Task<PaginatedList<CityDto>> GetAllCitiesAsync(int pageNumber, int pageSize)
    {
        return await _cityService.GetAllCitiesAsync(pageNumber, pageSize);
    }
}