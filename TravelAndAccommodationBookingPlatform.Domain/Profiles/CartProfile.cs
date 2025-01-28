using AutoMapper;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Models.CartDtos;

namespace TravelAndAccommodationBookingPlatform.Domain.Profiles;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<AddToCartDto, Cart>();
        CreateMap<Cart, CartDto>();
    }
}