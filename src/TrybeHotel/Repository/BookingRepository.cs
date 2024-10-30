using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;
using TrybeHotel.Utils;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace TrybeHotel.Repository;

public class BookingRepository : IBookingRepository {
    protected readonly ITrybeHotelContext _context;
    protected readonly IGetModel _getModel;

    public BookingRepository(ITrybeHotelContext context, IGetModel getModel) {
        _context = context;
        _getModel = getModel;
    }

    public BookingResponse AddBooking(BookingDtoInsert booking, int userId) {
        // Verifica se o quarto existe
        Room? room = _context.Rooms
            .Include(r => r.Bookings)
            .SingleOrDefault(r => r.RoomId == booking.RoomId);
        if (room == null) throw new RoomNotFoundException();

        // Verifica se o quarto tem capacidade suficiente
        if (booking.GuestQuant > room.Capacity) throw new RoomCapacityExceededException();

        // Verfica se as datas de check-in e check-out são validas
        if (
            booking.CheckIn.Date <= DateTime.Today ||
            booking.CheckOut.Date <= booking.CheckIn.Date
        ) {
            throw new InvalidBookingDateException(booking.CheckIn, booking.CheckOut);
        }

        // Verifica se o quarto já possui alguma reserva no periodo
        bool isRoomReserved = room.Bookings?.Any(b =>
            (b.CheckIn < booking.CheckOut.AddHours(-2)) &&
            (booking.CheckIn.AddHours(2) < b.CheckOut)
        ) ?? false;
        if (isRoomReserved) throw new RoomUnavailableException(room.Name);

        Booking newBooking = new Booking {
            CheckIn = booking.CheckIn.Date,
            CheckOut = booking.CheckOut.Date,
            GuestQuant = booking.GuestQuant,
            RoomId = room.RoomId,
            UserId = userId,
        };

        _context.Bookings.Add(newBooking);
        _context.SaveChanges();

        RoomDto roomDto = new RoomDto() {
            RoomId = room.RoomId,
            Name = room.Name,
            Capacity = room.Capacity,
            Image = room.Image,
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

        roomDto.Hotel = hotelDto;
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
            RoomId = room.RoomId,
            Name = room.Name,
            Capacity = room.Capacity,
            Image = room.Image,
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

        roomDto.Hotel = hotelDto;
        BookingResponse bookingDto = SimpleMapper.Map<Booking, BookingResponse>(booking);
        bookingDto.Room = roomDto;

        return bookingDto;
    }

    public Room GetRoomById(int RoomId) {
        return _getModel.Room(RoomId);
    }

}
