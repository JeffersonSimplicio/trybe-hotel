namespace TrybeHotel.Exceptions;

public class HotelNotFoundException : NotFoundException {
    public HotelNotFoundException()
        : base("Hotel not found.") { }
    public HotelNotFoundException(string hotelName)
        : base($"The hotel, {hotelName}, was not found.") { }
}
