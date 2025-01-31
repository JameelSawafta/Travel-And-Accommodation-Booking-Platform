using FluentValidation;
using TravelAndAccommodationBookingPlatform.Domain.Models.SearchDtos;

namespace TravelAndAccommodationBookingPlatform.API.Validators.ModelsValidators.HomeValidators;


public class SearchHotelsValidator : GenericValidator<SearchRequestDto>
{
    public SearchHotelsValidator()
    {
        RuleFor(x => x.Query).NotEmpty().WithMessage("Query is required");
        RuleFor(x => x.CheckInDate)
            .LessThan(x => x.CheckOutDate).WithMessage("CheckInDate must be less than CheckOutDate")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("CheckInDate must be greater than or equal to today")
            .Must(x => DateTime.TryParse(x.ToString(), out _)).WithMessage("CheckInDate must be a valid date");
        RuleFor(x => x.CheckOutDate)
            .GreaterThan(x => x.CheckInDate).WithMessage("CheckOutDate must be greater than CheckInDate")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("CheckOutDate must be greater than or equal to today")
            .Must(x => DateTime.TryParse(x.ToString(), out _)).WithMessage("CheckOutDate must be a valid date");
        RuleFor(x => x.Adults).NotEmpty().WithMessage("Adults should not be empty")
            .GreaterThan(0).WithMessage("Adults must be greater than 0");
        RuleFor(x => x.Children).NotEmpty().WithMessage("Children should not be empty")
            .GreaterThanOrEqualTo(0).WithMessage("Children must be greater than or equal to 0");
        RuleFor(x => x.Rooms).NotEmpty().WithMessage("Rooms should not be empty")
            .GreaterThan(0).WithMessage("Rooms must be greater than 0");
    }
}