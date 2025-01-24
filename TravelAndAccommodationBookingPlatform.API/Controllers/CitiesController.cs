using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.API.Validators.CityValidators;
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
    
    /// <summary>
    /// Retrieves a city by name.
    /// </summary>
    /// <param name="cityName">The name of the city.</param>
    /// <returns>A city.</returns>
    /// <response code="200">Returns the city.</response>
    /// <response code="404">If the city is not found.</response>
    [HttpGet("{cityName}")]
    public async Task<CityDto> GetCityByNameAsync(string cityName)
    {
        return await _cityService.GetCityByNameAsync(cityName);
    }
    
    /// <summary>
    /// Retrieves a city by ID.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <returns>A city.</returns>
    /// <response code="200">Returns the city.</response>
    /// <response code="404">If the city is not found.</response>
    [HttpGet("{cityId:guid}")]
    public async Task<CityDto> GetCityByIdAsync(Guid cityId)
    {
        return await _cityService.GetCityByIdAsync(cityId);
    }
    
    /// <summary>
    /// Creates a new city.
    /// </summary>
    /// <param name="cityDto"></param>
    /// <returns> A response with status code 201 (Created).</returns>
    /// <response code="201">Returns a response with status code 201 (Created).</response>
    /// <response code="400">If the city creation request is invalid.</response>
    /// <response code="409">If the city already exists.</response>
    [HttpPost]
    public async Task<IActionResult> CreateCityAsync(CreateCityDto cityDto)
    {
        var validator = new CreateCityValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(cityDto);
        await _cityService.CreateCityAsync(cityDto);
        return Created();
    }
}