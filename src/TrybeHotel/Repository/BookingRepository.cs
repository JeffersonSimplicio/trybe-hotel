using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;
using TrybeHotel.Utils;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository;

public class BookingRepository : IBookingRepository {
    protected readonly ITrybeHotelContext _context;
    protected readonly IGetModel _getModel;

    public BookingRepository(
        ITrybeHotelContext context,
        IGetModel getModel
    ) {
        _context = context;
        _getModel = getModel;
    }

    // 9. Refatore o endpoint POST /booking
    public BookingResponse Add(BookingDtoInsert booking, string email) {
        Room room = GetRoomById(booking.RoomId);
        User user = _getModel.User(email);

        if (booking.GuestQuant > room.Capacity) throw new RoomCapacityExceededException();

        Booking newBooking = new Booking {
            CheckIn = booking.CheckIn,
            CheckOut = booking.CheckOut,
            GuestQuant = booking.GuestQuant,
            RoomId = room.RoomId,
            UserId = user.UserId,
        };

        _context.Bookings.Add(newBooking);
        _context.SaveChanges();

        RoomDto roomDto = new RoomDto() {
            roomId = room.RoomId,
            name = room.Name,
            capacity = room.Capacity,
            image = room.Image,
        };

        HotelDto hotelDto = _context.Hotels
            .Where(h => h.HotelId == room.HotelId)
            .Include(h => h.City)
            .Select(h => new HotelDto {
                HotelId = h.HotelId,
                Name = h.Name,
                Address = h.Address,
                CityId = h.CityId,
                CityName = h.City!.Name,
                State = h.City.State,
            })
            .First();

        roomDto.hotel = hotelDto;
        BookingResponse bookingDto = SimpleMapper.Map<Booking, BookingResponse>(newBooking);
        bookingDto.Room = roomDto;

        return bookingDto;
    }

    // 10. Refatore o endpoint GET /booking
    public BookingResponse GetBooking(int bookingId, string email) {
        User user = _getModel.User(email);
        Booking? booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
        if (booking == null) throw new BookingNotFoundException();

        if (booking.UserId != user.UserId) throw new UnauthorizedAccessException();

        Room room = GetRoomById(booking.RoomId);

        RoomDto roomDto = new RoomDto() {
            roomId = room.RoomId,
            name = room.Name,
            capacity = room.Capacity,
            image = room.Image,
        };

        HotelDto hotelDto = _context.Hotels
            .Where(h => h.HotelId == room.HotelId)
            .Include(h => h.City)
            .Select(h => new HotelDto {
                HotelId = h.HotelId,
                Name = h.Name,
                Address = h.Address,
                CityId = h.CityId,
                CityName = h.City!.Name,
                State = h.City.State,
            })
            .First();

        roomDto.hotel = hotelDto;
        BookingResponse bookingDto = SimpleMapper.Map<Booking, BookingResponse>(booking);
        bookingDto.Room = roomDto;

        return bookingDto;
    }

    public Room GetRoomById(int RoomId) {
        return _getModel.Room(RoomId);
    }

}
