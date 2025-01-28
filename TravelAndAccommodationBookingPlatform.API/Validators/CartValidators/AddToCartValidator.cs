using FluentValidation;
using TravelAndAccommodationBookingPlatform.Domain.Models.CartDtos;

namespace TravelAndAccommodationBookingPlatform.API.Validators.CartValidators;

public class AddToCartValidator : GenericValidator<AddToCartDto>
{
    public AddToCartValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("RoomId is required");
        RuleFor(x => x.CheckInDate)
            .NotEmpty().WithMessage("CheckInDate is required")
            .LessThan(x => x.CheckOutDate).WithMessage("CheckInDate must be less than CheckOutDate")
            .GreaterThan(DateTime.Now).WithMessage("CheckInDate must be greater than current date");
        RuleFor(x => x.CheckOutDate)
            .NotEmpty().WithMessage("CheckOutDate is required")
            .GreaterThan(x => x.CheckInDate).WithMessage("CheckOutDate must be greater than CheckInDate")
            .GreaterThan(DateTime.Now).WithMessage("CheckOutDate must be greater than current date");
    }
}