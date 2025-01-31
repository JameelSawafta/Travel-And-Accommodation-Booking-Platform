using FluentValidation;
using TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;

namespace TravelAndAccommodationBookingPlatform.API.Validators.ModelsValidators.RoomValidators;

public class CreateRoomValidator : GenericValidator<CreateRoomDto>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("HotelId is required");
        RuleFor(x => x.RoomNumber)
            .NotEmpty().WithMessage("RoomNumber is required")
            .MaximumLength(10).WithMessage("RoomNumber cannot be more than 10 characters");
        RuleFor(x => x.PricePerNight)
            .NotEmpty().WithMessage("PricePerNight is required");
        RuleFor(x => x.RoomType)
            .NotEmpty().WithMessage("RoomType is required");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot be more than 500 characters");
        RuleFor(x => x.AdultsCapacity)
            .NotEmpty().WithMessage("AdultsCapacity is required");
        RuleFor(x => x.ChildrenCapacity)
            .NotEmpty().WithMessage("ChildrenCapacity is required");
        RuleFor(x => x.Availability)
            .NotEmpty().WithMessage("Availability is required");
    }
}