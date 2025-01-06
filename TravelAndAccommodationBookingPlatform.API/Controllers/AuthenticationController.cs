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

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="loginDto">The login details (username and password).</param>
    /// <returns>A JWT token string for the authenticated user.</returns>
    /// <response code="200">Returns the JWT token for successful login.</response>
    /// <response code="400">If the login request is invalid.</response>
    /// <response code="401">If authentication fails.</response>
    [HttpPost("login")]
    public async Task<string> Login([FromBody] LoginDto loginDto)
    {
        var validator = new LoginValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(loginDto);
        
        return await _authService.LoginAsync(loginDto);
    }

    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="signupDto">The user details for registration.</param>
    /// <returns>A 201 status code upon successful registration.</returns>
    /// <response code="201">User registered successfully.</response>
    /// <response code="400">If the signup request is invalid.</response>
    /// <response code="409">If the username or email already exists.</response>
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupDto signupDto)
    {
        var validator = new SignupValidator();
        await validator.ValidateAndThrowCustomExceptionAsync(signupDto);

        await _authService.SignupAsync(signupDto);
        
        return Created();
    }
}