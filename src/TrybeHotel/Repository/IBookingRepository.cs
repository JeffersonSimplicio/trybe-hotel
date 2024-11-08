using TrybeHotel.Models;
using TrybeHotel.Dto;
using System.Drawing.Printing;

namespace TrybeHotel.Repository;

public interface IBookingRepository {
    BookingDto GetBookingById(int bookingId, int userId, string userType);
    IEnumerable<BookingDto> GetAllBookings(
        int pageNumber,
        int pageSize,
        DateTime? startDate = null,
        DateTime? endDate = null
    );
    //Implementar FindBookingByUser
    BookingDto AddBooking(BookingCreateDto newBooking, int userId);
    BookingDto UpdateBooking(
        int bookingId,
        BookingUpdateDto updatedBooking,
        int userId
    );
    void DeleteBooking(int bookingId, int userId);
}