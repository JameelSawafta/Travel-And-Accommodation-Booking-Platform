using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Discount
{
    [Key]
    public Guid DiscountId { get; set; } = Guid.NewGuid();
    [MaxLength(500)]
    public string? Description { get; set; }
    [Required]
    [MaxLength(20)]
    public string Code { get; set; }
    [Required]
    public DiscountType DiscountType { get; set; }
    [Required]
    [Range(0, 100)]
    public double DiscountValue { get; set; }
    [Required]
    [Column(TypeName = "timestamp")]
    public DateTime ValidFrom { get; set; }
    [Required]
    [Column(TypeName = "timestamp")]
    public DateTime ValidTo { get; set; }
    [Required]
    public bool IsActive { get; set; }
}