using System.ComponentModel.DataAnnotations;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Room
{
    [Key]
    public Guid RoomId { get; set; } = Guid.NewGuid();
    [Required]
    public Guid HotelId { get; set; }
    [Required]
    [MaxLength(10)]
    public string RoomNumber { get; set; }
    [Required]
    [Range(0, 10000)]
    public decimal Price { get; set; }
    [Required]
    public RoomType RoomType { get; set; }
    [MaxLength(500)]
    public string Description { get; set; }
    [Required]
    [Range(1, 10)]
    public int AdultsCapacity { get; set; }
    [Required]
    [Range(0, 10)]
    public int ChildrenCapacity { get; set; }
    [Required]
    public bool Availability { get; set; }

    public Hotel Hotel { get; set; }
}