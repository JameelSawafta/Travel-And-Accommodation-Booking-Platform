using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.API.Validators.AuthValidators;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Domain.Models.UserDtos;

namespace TravelAndAccommodationBookingPlatform.API.Controllers;

[ApiController]
[Route("api/authentication")]
[ApiVersion("1.0")]
public class AuthenticationController : Controller
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<string> Login([FromBody] LoginDto loginDto)
    {
        var validator = new LoginValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(loginDto);
        
        var response = await _authService.LoginAsync(loginDto);
        return response;
    }

    [HttpPost("signup")]
    public async Task<UserCreationResponseDto> Signup([FromBody] SignupDto signupDto)
    {
        var validator = new SignupValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(signupDto);
        
        var response = await _authService.SignupAsync(signupDto);
        return response;
    }
}