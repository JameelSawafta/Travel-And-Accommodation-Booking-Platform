namespace TravelAndAccommodationBookingPlatform.Domain.Exceptions;

public class RequestValidationException : Exception
{
    public Dictionary<string, List<string>> Errors { get; }

    public RequestValidationException(string message, Dictionary<string, List<string>> errors) : base(message)
    {
        Errors = errors;
    }
}