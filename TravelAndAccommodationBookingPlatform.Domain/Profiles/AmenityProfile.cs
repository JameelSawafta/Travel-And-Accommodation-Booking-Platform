using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.AmenityDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Profiles;

public class AmenityProfile : Profile
{
    public AmenityProfile()
    {
        CreateMap<Amenity, AmenityDto>();
    }
}