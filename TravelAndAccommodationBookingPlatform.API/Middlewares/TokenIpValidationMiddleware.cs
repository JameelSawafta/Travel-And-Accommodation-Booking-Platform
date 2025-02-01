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
                return;
            }

            string requestIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                               ?? context.Connection.RemoteIpAddress?.ToString()
                               ?? "Unknown";

            if (requestIp == "Unknown")
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }


            if (!requestIp.Equals(tokenIp, StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
        }

        await _next(context);
    }
}