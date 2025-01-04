using FluentValidation;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;

namespace TravelAndAccommodationBookingPlatform.API.Validators;

public class GenericValidator<T> : AbstractValidator<T>
{
    public async Task ValidateAndThrowCustomExceptionAsync(T request, string message = "Request validation failed")
    {
        var results = await ValidateAsync(request);

        if (!results.IsValid)
        {
            var errorDictionary = results.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(e => e.ErrorMessage).ToList()
                );

            throw new RequestValidationException(message,errorDictionary);
        }
    }
}