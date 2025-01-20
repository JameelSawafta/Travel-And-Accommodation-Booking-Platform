using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.CityDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Profiles;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, TrendingDestinationDto>();
    }
}