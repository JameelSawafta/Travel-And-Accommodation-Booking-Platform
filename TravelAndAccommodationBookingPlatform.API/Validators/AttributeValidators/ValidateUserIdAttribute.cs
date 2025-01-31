using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TravelAndAccommodationBookingPlatform.API.Validators.AttributeValidators;

public class ValidateUserIdAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;

        if (user.Identity?.IsAuthenticated != true)
        {
            context.Result = new UnauthorizedObjectResult("User is not authenticated.");
            return;
        }

        string tokenUserId = user.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(tokenUserId))
        {
            context.Result = new UnauthorizedObjectResult("Missing UserId in token.");
            return;
        }

        string requestUserId = GetUserIdFromRequest(context);
        if (string.IsNullOrEmpty(requestUserId))
        {
            context.Result = new BadRequestObjectResult("UserId is required in the request.");
            return;
        }

        if (!requestUserId.Equals(tokenUserId, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new ForbidResult(); 
            return;
        }

        base.OnActionExecuting(context);
    }

    private string GetUserIdFromRequest(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;

        if (httpContext.Request.RouteValues.TryGetValue("userId", out var routeUserId) && routeUserId is string routeId)
        {
            return routeId;
        }

        if (httpContext.Request.Query.TryGetValue("userId", out var queryUserId))
        {
            return queryUserId;
        }

        if (httpContext.Request.HasJsonContentType())
        {
            foreach (var arg in context.ActionArguments.Values)
            {
                if (arg != null)
                {
                    var userIdProperty = arg.GetType().GetProperty("UserId");

                    if (userIdProperty != null)
                    {
                        var userIdValue = userIdProperty.GetValue(arg)?.ToString();
                        if (!string.IsNullOrEmpty(userIdValue))
                        {
                            return userIdValue;
                        }
                    }
                }
            }
        }

        return null; 
    }
}