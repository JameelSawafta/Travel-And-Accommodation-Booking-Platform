namespace TravelAndAccommodationBookingPlatform.Domain.Models.SearchDtos;

/// <summary>
/// Represents the search criteria for hotels.
/// </summary>
public class SearchRequestDto
{
    /// <summary>
    /// The search query to find hotels by name or city name.
    /// </summary>
    public string? Query { get; set; }

    /// <summary>
    /// The check-in date. Default is today's date.
    /// </summary>
    public DateTime? CheckInDate { get; set; } = DateTime.Today;

    /// <summary>
    /// The check-out date. Default is tomorrow's date.
    /// </summary>
    public DateTime? CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

    /// <summary>
    /// The number of adults. Default is 2.
    /// </summary>
    public int? Adults { get; set; } = 2;

    /// <summary>
    /// The number of children. Default is 0.
    /// </summary>
    public int? Children { get; set; } = 0;

    /// <summary>
    /// The number of rooms. Default is 1.
    /// </summary>
    public int? Rooms { get; set; } = 1;
}