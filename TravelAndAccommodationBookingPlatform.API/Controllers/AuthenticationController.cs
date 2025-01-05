using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.API.Validators.AuthValidators;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;

namespace TravelAndAccommodationBookingPlatform.API.Controllers;

[ApiController]
[Route("api/auth")]
[ApiVersion("1.0")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<string> Login([FromBody] LoginDto loginDto)
    {
        var validator = new LoginValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(loginDto);
        
        return await _authService.LoginAsync(loginDto);
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupDto signupDto)
    {
        var validator = new SignupValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(signupDto);

        await _authService.SignupAsync(signupDto);
        
        return Created();
    }
}