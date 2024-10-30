using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IBookingRepository {
    BookingResponse AddBooking(BookingDtoInsert booking, int userId);
    BookingResponse GetBookingById(int bookingId, int userId, string userType);
    BookingResponse UpdateBooking(int bookingId, BookingDtoInsert updatedBooking, int userId);
}