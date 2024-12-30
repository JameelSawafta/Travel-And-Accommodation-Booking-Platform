using System.ComponentModel.DataAnnotations;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class User
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Range(0, 5)]
    public int Role { get; set; } = 0;
    [Required]
    [Phone]
    public string? PhoneNumber { get; set; }
    public Guid Salt { get; set; } = Guid.NewGuid();
}