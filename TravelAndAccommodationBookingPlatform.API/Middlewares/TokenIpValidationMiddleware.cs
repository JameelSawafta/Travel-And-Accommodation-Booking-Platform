using System.Net;

namespace TravelAndAccommodationBookingPlatform.API.Middlewares;

public class TokenIpValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenIpValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var user = context.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            string tokenIp = user.FindFirst("ClientIp")?.Value ?? "Unknown";

            if (tokenIp == "Unknown")
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Token IP is missing or invalid.");
                return;
            }

            string requestIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                               ?? context.Connection.RemoteIpAddress?.ToString()
                               ?? "Unknown";

            if (requestIp == "Unknown")
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Request IP is missing or invalid.");
                return;
            }


            if (!requestIp.Equals(tokenIp, StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("IP mismatch: Access denied.");
                return;
            }
        }

        await _next(context);
    }
}