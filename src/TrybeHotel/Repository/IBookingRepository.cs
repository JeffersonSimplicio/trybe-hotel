using TrybeHotel.Models;
using TrybeHotel.Dto;
using System.Drawing.Printing;

namespace TrybeHotel.Repository;

public interface IBookingRepository {
    BookingResponse AddBooking(BookingDtoInsert booking, int userId);
    BookingResponse GetBookingById(int bookingId, int userId, string userType);
    IEnumerable<BookingResponse> GetAllBookings(
        int pageNumber,
        int pageSize,
        DateTime? startDate = null,
        DateTime? endDate = null
    );
    BookingResponse UpdateBooking(int bookingId, BookingDtoInsert updatedBooking, int userId);
    void DeleteBooking(int bookingId, int userId);
}