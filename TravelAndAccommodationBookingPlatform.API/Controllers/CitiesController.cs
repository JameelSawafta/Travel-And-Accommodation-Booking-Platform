using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.API.Validators.CityValidators;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.Common;

namespace TravelAndAccommodationBookingPlatform.API.Controllers;

[ApiController]
[Route("api/cities")]
[ApiVersion("1.0")]
[Authorize(Policy = "AdminOnly")]
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
    /// <response code="400">If the page number or page size is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
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
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
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
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
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
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    /// <response code="409">If the city already exists.</response>
    [HttpPost]
    public async Task<IActionResult> CreateCityAsync(CreateCityDto cityDto)
    {
        var validator = new CreateCityValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(cityDto);
        await _cityService.CreateCityAsync(cityDto);
        return Created();
    }
    
    /// <summary>
    /// Updates a city.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <param name="cityDto">The updated city details.</param>
    /// <returns> A response with status code 204 (No Content).</returns>
    /// <response code="204">Returns a response with status code 204 (No Content).</response>
    /// <response code="400">If the city update request is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    /// <response code="404">If the city is not found.</response>
    [HttpPut("{cityId:guid}")]
    public async Task<IActionResult> UpdateCityAsync(Guid cityId, UpdateCityDto cityDto)
    {
        var validator = new UpdateCityValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(cityDto);
        await _cityService.UpdateCityAsync(cityId, cityDto);
        return NoContent();
    }
    
    /// <summary>
    /// Deletes a city.
    /// </summary>
    /// <param name="cityId">The ID of the city.</param>
    /// <returns> A response with status code 204 (No Content).</returns>
    /// <response code="204">Returns a response with status code 204 (No Content).</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    /// <response code="404">If the city is not found.</response>
    [HttpDelete("{cityId:guid}")]
    public async Task<IActionResult> DeleteCityAsync(Guid cityId)
    {
        await _cityService.DeleteCityAsync(cityId);
        return NoContent();
    }
}