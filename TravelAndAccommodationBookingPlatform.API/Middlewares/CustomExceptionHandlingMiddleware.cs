using System.Text.Json;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;

namespace TravelAndAccommodationBookingPlatform.API.Middlewares;

public class CustomExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(context, ex, 404);
        }
        catch (RequestValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (AuthenticationFailedException ex)
        {
            await HandleExceptionAsync(context, ex, 401);
        }
        catch (ConflictException ex)
        {
            await HandleExceptionAsync(context, ex, 409);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, 500);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var errorResponse = new
        {
            error = exception.Message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, RequestValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 400;

        var errorResponse = new
        {
            title = exception.Message,
            errors = exception.Errors
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}