using System.ComponentModel.DataAnnotations;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class City
{
    [Key]
    public Guid CityId { get; set; } = Guid.NewGuid();
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(100)]
    public string Country { get; set; }
    [Required]
    [MaxLength(20)]
    public string PostOfficeCode { get; set; }
    [MaxLength(20)]
    public string? PostCode { get; set; }
}