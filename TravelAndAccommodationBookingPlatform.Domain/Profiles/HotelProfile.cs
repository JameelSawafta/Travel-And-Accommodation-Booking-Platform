using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.AmenityDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.HotelDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<Hotel, HotelSearchResultDto>()
            .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.HotelId))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.HotelName))
            .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.StarRating))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.CityName))
            .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));
    }
}