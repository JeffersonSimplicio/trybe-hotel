using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Utils;
using TrybeHotel.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security;

namespace TrybeHotel.Repository;

public class RoomRepository : IRoomRepository {
    protected readonly ITrybeHotelContext _context;
    protected readonly IGetModel _getModel;

    public RoomRepository(ITrybeHotelContext context, IGetModel getModel) {
        _context = context;
        _getModel = getModel;
    }

    private HotelDto GetHotelDtoById(int HotelId) {
        HotelDto? selectedHotel = (from hotel in _context.Hotels
                                   join city in _context.Cities
                                   on hotel.CityId equals city.CityId
                                   where hotel.HotelId == HotelId
                                   select new HotelDto {
                                       HotelId = hotel.HotelId,
                                       Name = hotel.Name,
                                       Address = hotel.Address,
                                       CityId = city.CityId,
                                   }).SingleOrDefault();

        if (selectedHotel == null) throw new HotelNotFoundException();

        return selectedHotel;
    }

    public RoomDto GetRoomById(int roomId) {
        Room room = _getModel.Room(roomId);

        return new RoomDto {
            RoomId = room.RoomId,
            Name = room.Name,
            Capacity = room.Capacity,
            Image = room.Image,
            HotelId = room.HotelId,
        };
    }

    public IEnumerable<RoomDto> GetAllRooms(int page, int size) {
        int skip = (page - 1) * size;

        return _context.Rooms
            .Skip(skip)
            .Take(size)
            .Select(r => new RoomInfoDto {
                RoomId = r.RoomId,
                Name = r.Name,
                Capacity = r.Capacity,
                Image = r.Image,
                HotelId = r.HotelId,
            });
    }

    public RoomDto AddRoom(RoomCreateDto newRoom) {
        Hotel? hotel = _context.Hotels.SingleOrDefault(h => h.HotelId == newRoom.HotelId);
        if (hotel == null) throw new HotelNotFoundException();

        Room room = new Room() {
            HotelId = hotel.HotelId,
            Name = newRoom.Name,
            Capacity = newRoom.Capacity,
            Image = newRoom.Image,
        };

        _context.Rooms.Add(room);
        _context.SaveChanges();

        RoomDto newRoom = new RoomDto {
            RoomId = room.RoomId,
            Name = room.Name,
            Capacity = room.Capacity,
            Image = room.Image,
            HotelId = room.HotelId,
        };
        return newRoom;
    }

    public RoomDto UpdateRoom(int roomId, RoomUpdateDto updateRoom) {
        // VerificationException se o quarto existe
        Room room = _getModel.Room(roomId);

        // Verifica se o hotel existe
        GetHotelDtoById(updateRoom.HotelId);

        room.HotelId = updateRoom.HotelId;
        room.Name = updateRoom.Name;
        room.Capacity = updateRoom.Capacity;
        room.Image = updateRoom.Image;

        _context.SaveChanges();

        return new RoomDto {
            RoomId = room.RoomId,
            Name = updateRoom.Name,
            Capacity = updateRoom.Capacity,
            Image = updateRoom.Image,
            HotelId = room.HotelId,
        };
    }

    public void DeleteRoom(int roomId) {
        Room room = _getModel.Room(roomId);
        _context.Rooms.Remove(room);
        _context.SaveChanges();
    }
}