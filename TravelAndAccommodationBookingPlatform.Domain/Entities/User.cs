using System.ComponentModel.DataAnnotations;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class User
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public UserRole Role { get; set; } = 0;
    [Required]
    [Phone]
    public string? PhoneNumber { get; set; }
    public Guid Salt { get; set; } = Guid.NewGuid();
}