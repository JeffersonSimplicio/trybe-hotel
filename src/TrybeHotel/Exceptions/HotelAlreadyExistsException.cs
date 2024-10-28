namespace TrybeHotel.Exceptions;

public class HotelAlreadyExistsException : EntityAlreadyExistsException{
    public HotelAlreadyExistsException()
        : base("A hotel with the same name already exists in this city.") { }

    public HotelAlreadyExistsException(string hotelName, string cityName)
        : base($"Hotel with name '{hotelName}' already exists in city '{cityName}'.") {
    }
}
