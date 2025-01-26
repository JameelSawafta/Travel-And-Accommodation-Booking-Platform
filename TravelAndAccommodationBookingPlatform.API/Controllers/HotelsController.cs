using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.API.Validators.HotelValidators;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.Common;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;

namespace TravelAndAccommodationBookingPlatform.API.Controllers;

[ApiController]
[Route("api/hotels")]
[ApiVersion("1.0")]
[Authorize(Policy = "AdminOnly")]
public class HotelsController : Controller
{
    private readonly IHotelService _hotelService;
    
    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }
    
    /// <summary>
    /// Retrieves a paginated list of all hotels.
    /// </summary>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <returns>A paginated list of hotels.</returns>
    /// <response code="200">Returns the paginated list of hotels.</response>
    /// <response code="400">If the page number or page size is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    [HttpGet]
    public async Task<PaginatedList<HotelDto>> GetHotelsAsync(int pageSize, int pageNumber)
    {
        return await _hotelService.GetAllHotelsAsync(pageNumber, pageSize);
    }
    
    /// <summary>
    /// Retrieves a hotel by ID.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel.</param>
    /// <returns>A hotel.</returns>
    /// <response code="200">Returns the hotel.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    /// <response code="404">If the hotel is not found.</response>
    [HttpGet("{hotelId}")]
    public async Task<HotelDto> GetHotelByIdAsync(Guid hotelId)
    {
        return await _hotelService.GetHotelByIdAsync(hotelId);
    }
    
    /// <summary>
    /// Creates a new hotel.
    /// </summary>
    /// <param name="hotelDto">The hotel to create.</param>
    /// <returns>A response with status code 201 (Created).</returns>
    /// <response code="201">Returns the created hotel.</response>
    /// <response code="400">If the hotel is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    [HttpPost]
    public async Task<IActionResult> CreateHotelAsync(CreateHotelDto hotelDto)
    {
        var validator = new CreateHotelValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(hotelDto);
        await _hotelService.CreateHotelAsync(hotelDto);
        return Created();
    }
    
    /// <summary>
    /// Updates a hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel to update.</param>
    /// <param name="hotelDto">The hotel to update.</param>
    /// <returns>A response with status code 204 (No Content).</returns>
    /// <response code="204">Returns a response with status code 204 (No Content).</response>
    /// <response code="400">If the hotel is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    /// <response code="404">If the hotel is not found.</response>
    [HttpPut("{hotelId}")]
    public async Task<IActionResult> UpdateHotelAsync(Guid hotelId, UpdateHotelDto hotelDto)
    {
        var validator = new UpdateHotelValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(hotelDto);
        await _hotelService.UpdateHotelAsync(hotelId, hotelDto);
        return NoContent();
    }
    
    /// <summary>
    /// Deletes a hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel to delete.</param>
    /// <returns>A response with status code 204 (No Content).</returns>
    /// <response code="204">Returns a response with status code 204 (No Content).</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is not authorized.</response>
    [HttpDelete("{hotelId}")]
    public async Task<IActionResult> DeleteHotelAsync(Guid hotelId)
    {
        await _hotelService.DeleteHotelAsync(hotelId);
        return NoContent();
    }
}