using System.ComponentModel.DataAnnotations;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Hotel
{
    [Key]
    public Guid HotelId { get; set; } = Guid.NewGuid();
    [Required]
    public Guid CityId { get; set; }
    [Required]
    [MaxLength(100)]
    public string HotelName { get; set; }
    [MaxLength(500)]
    public string Description { get; set; }
    [Required]
    [Range(1, 5)]
    public int StarRating { get; set; }
    [MaxLength(100)]
    public string? Owner { get; set; }
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
    [Required]
    [MaxLength(200)]
    public string Address { get; set; }

    public City City { get; set; }
}