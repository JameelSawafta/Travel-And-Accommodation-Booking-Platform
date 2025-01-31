using FluentValidation;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;

namespace TravelAndAccommodationBookingPlatform.API.Validators.ModelsValidators.HotelValidators;

public class CreateHotelValidator : GenericValidator<CreateHotelDto>
{
    public CreateHotelValidator()
    {
        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("CityId is required");
        RuleFor(x => x.HotelName)
            .NotEmpty().WithMessage("HotelName is required")
            .MaximumLength(100).WithMessage("HotelName cannot be more than 100 characters");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot be more than 500 characters");
        RuleFor(x => x.StarRating)
            .NotEmpty().WithMessage("StarRating is required")
            .InclusiveBetween(1, 5).WithMessage("StarRating must be between 1 and 5");
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required");
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(200).WithMessage("Address cannot be more than 200 characters");
        RuleFor(x => x.Latitude)
            .NotEmpty().WithMessage("Latitude is required");
        RuleFor(x => x.Longitude)
            .NotEmpty().WithMessage("Longitude is required");
        RuleFor(x => x.ThumbnailUrl)
            .MaximumLength(200).WithMessage("ThumbnailUrl cannot be more than 200 characters");
    }
}