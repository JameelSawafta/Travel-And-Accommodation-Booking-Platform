using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.AmenityDtos;
using TravelAndAccommodationBookingPlatform.Domain.Models.RoomDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Profiles;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<RoomAmenity, RoomAmenityDto>();
        CreateMap<RoomDiscount, RoomDiscountDto>();

        
        CreateMap<Room, RoomDetailedDto>()
            .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.RoomNumber))
            .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.PricePerNight))
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.AdultsCapacity, opt => opt.MapFrom(src => src.AdultsCapacity))
            .ForMember(dest => dest.ChildrenCapacity, opt => opt.MapFrom(src => src.ChildrenCapacity))
            .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => src.Availability))
            .ForMember(dest => dest.RoomAmenities, opt => opt.MapFrom(src => src.RoomAmenities.Select(ra => ra.Amenity)))
            .ForMember(dest => dest.DiscountPercentageValue, opt => opt.MapFrom(src => src.RoomDiscounts.FirstOrDefault().Discount.DiscountPercentageValue))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));
        CreateMap<Room, RoomDto>()
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()));
        CreateMap<CreateRoomDto, Room>();
        CreateMap<UpdateRoomDto, Room>();
    }
}