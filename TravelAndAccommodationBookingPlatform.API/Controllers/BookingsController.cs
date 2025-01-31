using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.API.Validators.AttributeValidators;
using TravelAndAccommodationBookingPlatform.API.Validators.ModelsValidators.BookingValidators;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models;

namespace TravelAndAccommodationBookingPlatform.API.Controllers;

[ApiController]
[Route("api/bookings")]
[ApiVersion("1.0")]
[Authorize(Policy = "UserOrAdmin")]
public class BookingsController : Controller
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    
    /// <summary>
    /// move the cart items to the booking and create payment
    /// </summary>
    /// <param name="requestDto"> the checkout request </param>
    /// <returns> the approval url and the booking id </returns>
    /// <response code="200">returns the approval url and the booking id</response>
    /// <response code="401">if the user is not authenticated</response>
    /// <response code="403">if the user id in the token does not match the user id in the request</response>
    /// <response code="404">if the user id is not valid or the user does not have a cart</response>
    /// <response code="409">if the room is not available for the selected dates</response>
    [ValidateUserId]
    [HttpPost("checkout")]
    public async Task<CheckoutDto> CheckoutAsync([FromBody] CheckoutRequestDto requestDto)
    {
        var validator = new CheckoutValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(requestDto);
        return await _bookingService.CreateBookingFromCartAsync(requestDto);
    }
}