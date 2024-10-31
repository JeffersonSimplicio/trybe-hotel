using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;
using TrybeHotel.Utils;
using Microsoft.EntityFrameworkCore;
using System.Security;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        ValidateBookingDates(booking);

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

        HotelDto hotelDto = GetHotelDtoById(room.HotelId);

        roomDto.Hotel = hotelDto;
        BookingResponse bookingDto = SimpleMapper.Map<Booking, BookingResponse>(newBooking);
        bookingDto.Room = roomDto;

        return bookingDto;
    }

    public BookingResponse GetBookingById(int bookingId, int userId, string userType) {
        Booking? booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
        if (booking == null) throw new BookingNotFoundException();

        User user = _getModel.User(userId);

        if (userType.ToLower() == "client" && booking.UserId != user.UserId) {
            throw new UnauthorizedAccessException();
        }

        Room room = _getModel.Room(booking.RoomId);

        RoomDto roomDto = new RoomDto() {
            RoomId = room.RoomId,
            Name = room.Name,
            Capacity = room.Capacity,
            Image = room.Image,
        };

        HotelDto hotelDto = GetHotelDtoById(room.HotelId);

        roomDto.Hotel = hotelDto;
        BookingResponse bookingDto = SimpleMapper.Map<Booking, BookingResponse>(booking);
        bookingDto.Room = roomDto;

        return bookingDto;
    }

    public IEnumerable<BookingResponse> GetAllBookings(
        int pageNumber,
        int pageSize,
        DateTime? startDate = null,
        DateTime? endDate = null
    ) {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate) {
            throw new ArgumentException("A data de início não pode ser posterior à data de fim.");
        }
        // Define a consulta inicial para obter as reservas
        IQueryable<Booking> query = _context.Bookings;

        // Aplica o filtro de data inicial, se fornecido
        if (startDate.HasValue) {
            query = query.Where(b => b.CheckIn.Date >= startDate.Value.Date);
        }

        // Aplica o filtro de data final, se fornecido
        if (endDate.HasValue) {
            query = query.Where(b => b.CheckIn.Date <= endDate.Value.Date);
        }

        // Paginação: salta os registros das páginas anteriores e retorna no máximo n registros
        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        // Executa a consulta e projeta os resultados em BookingResponse
        var bookings = query.ToList();

        var bookingResponses = bookings.Select(booking => {
            Room room = _getModel.Room(booking.RoomId);
            RoomDto roomDto = new RoomDto {
                RoomId = room.RoomId,
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                Hotel = GetHotelDtoById(room.HotelId)
            };

            BookingResponse bookingDto = SimpleMapper.Map<Booking, BookingResponse>(booking);
            bookingDto.Room = roomDto;

            return bookingDto;
        });

        return bookingResponses;
    }

    public BookingResponse UpdateBooking(
        int bookingId,
        BookingDtoInsert updatedBooking,
        int userId
    ) {
        // Verifica se a reserva existe e obtem os dados da reserva
        Booking? existingBooking = _context.Bookings
            .SingleOrDefault(b => b.BookingId == bookingId);
        if (existingBooking == null) throw new BookingNotFoundException();

        // Verifica se o usuário é o dono da reserva
        if (existingBooking.UserId != userId) throw new UnauthorizedAccessException();

        //Verifica se o quato existe e obtem os dados do quarto
        Room? room = _context.Rooms
            .Include(r => r.Bookings)
            .SingleOrDefault(r => r.RoomId == updatedBooking.RoomId);

        if (room == null) throw new RoomNotFoundException();

        // Verifica se o quarto tem capacidade suficiente
        if (updatedBooking.GuestQuant > room.Capacity) throw new RoomCapacityExceededException();

        // Verifica se as datas de check-in e check-out são válidas
        ValidateBookingDates(updatedBooking);

        // Verifica se o quarto já possui alguma reserva no período (ignorando a reserva atual)
        bool isRoomReserved = room.Bookings?.Any(b =>
            b.BookingId != bookingId &&
            (b.CheckIn < updatedBooking.CheckOut.AddHours(-2)) &&
            (updatedBooking.CheckIn.AddHours(2) < b.CheckOut)
        ) ?? false;

        if (isRoomReserved) throw new RoomUnavailableException(room.Name);

        // Atualiza os dados da reserva
        existingBooking.CheckIn = updatedBooking.CheckIn.Date;
        existingBooking.CheckOut = updatedBooking.CheckOut.Date;
        existingBooking.GuestQuant = updatedBooking.GuestQuant;
        existingBooking.RoomId = room.RoomId;

        _context.SaveChanges();

        // Cria a resposta com os dados atualizados
        RoomDto roomDto = new RoomDto() {
            RoomId = room.RoomId,
            Name = room.Name,
            Capacity = room.Capacity,
            Image = room.Image,
        };

        HotelDto hotelDto = GetHotelDtoById(room.HotelId);
        roomDto.Hotel = hotelDto;

        BookingResponse bookingResponse = SimpleMapper.Map<Booking, BookingResponse>(existingBooking);
        bookingResponse.Room = roomDto;

        return bookingResponse;
    }

    public void DeleteBooking(int bookingId, int userId) {
        // Verifica se a reserva existe
        Booking? booking = _context.Bookings.SingleOrDefault(b => b.BookingId == bookingId);
        if (booking == null) throw new BookingNotFoundException();

        // Verifica se o usuário é o dono da reserva
        if (booking.UserId != userId) throw new UnauthorizedAccessException();

        DateTime now = DateTime.Now;
        // Verifica se a reserva já terminou ou está em andamento
        if (booking.CheckOut <= now) {
            throw new InvalidOperationException("A reserva não pode ser excluída pois já foi concluída.");
        }
        if (booking.CheckIn <= now && booking.CheckOut >= now) {
            throw new InvalidOperationException("A reserva não pode ser excluída pois está em andamento.");
        }

        // Verifica se faltam menos de 7 dias para a reserva
        if ((booking.CheckIn - now).TotalDays < 7) {
            throw new InvalidOperationException("A reserva só pode ser excluída com 7 dias de antecedência.");
        }

        // Remove a reserva
        _context.Bookings.Remove(booking);
        _context.SaveChanges();
    }

    private HotelDto GetHotelDtoById(int hotelId) {
        return _context.Hotels
            .Where(h => h.HotelId == hotelId)
            .Include(h => h.City)
            .Select(h => new HotelDto {
                HotelId = h.HotelId,
                Name = h.Name,
                Address = h.Address,
                CityId = h.CityId,
                CityName = h.City!.Name,
                State = h.City.State,
            })
            .Single();
    }

    private static void ValidateBookingDates(BookingDtoInsert booking) {
        if (
            booking.CheckIn.Date <= DateTime.Today ||
            booking.CheckOut.Date <= booking.CheckIn.Date
        ) {
            throw new InvalidBookingDateException(booking.CheckIn, booking.CheckOut);
        }
    }
}
