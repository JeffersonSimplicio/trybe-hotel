namespace TrybeHotel.Exceptions;

public class CityNotFoundException : NotFoundException {
    public CityNotFoundException()
        : base("City not found.") { }
    public CityNotFoundException(string cityName)
        : base($"The city, {cityName}, was not found.") { }
}
