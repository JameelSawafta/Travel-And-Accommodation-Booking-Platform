using FluentValidation;
using TravelAndAccommodationBookingPlatform.Domain.Models;

namespace TravelAndAccommodationBookingPlatform.API.Validators.BookingValidators;

public class CheckoutValidator : GenericValidator<CheckoutRequestDto>
{
    public CheckoutValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
    }
}