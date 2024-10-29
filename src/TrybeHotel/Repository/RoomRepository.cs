using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Utils;
using TrybeHotel.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace TrybeHotel.Repository;

public class RoomRepository : IRoomRepository {
    protected readonly ITrybeHotelContext _context;
    protected readonly IGetModel _getModel;

    public RoomRepository(ITrybeHotelContext context, IGetModel getModel) {
        _context = context;
        _getModel = getModel;
    }

    private HotelDto GetHotelById(int HotelId) {
        HotelDto selectedHotel = (from hotel in _context.Hotels
                                  join city in _context.Cities
                                  on hotel.CityId equals city.CityId
                                  where hotel.HotelId == HotelId
                                  select new HotelDto {
                                      HotelId = hotel.HotelId,
                                      Name = hotel.Name,
                                      Address = hotel.Address,
                                      CityId = city.CityId,
                                      CityName = city.Name,
                                      State = city.State,
                                  }).First();
        return selectedHotel;
    }

    public RoomDto GetRoomById(int roomId) {
        Room? room = _context.Rooms.SingleOrDefault(r => r.RoomId == roomId);
        if (room == null) throw new RoomNotFoundException();

        return new RoomDto {
            RoomId = room.RoomId,
            Name = room.Name,
            Capacity = room.Capacity,
            Image = room.Image,
            Hotel = GetHotelById(room.HotelId),
        };
    }

    public IEnumerable<RoomInfoDto> GetAllRooms(int page, int size) {
        int skip = (page - 1) * size;

        return _context.Rooms
            .Skip(skip)
            .Take(size)
            .Select(r => new RoomInfoDto {
                RoomId = r.RoomId,
                Name = r.Name,
                Capacity = r.Capacity,
                Image = r.Image,
            });
    }

    public RoomDto AddRoom(RoomInsertDto roomInsert) {
        Hotel? hotel = _context.Hotels.SingleOrDefault(h => h.HotelId == roomInsert.HotelId);
        if (hotel == null) throw new HotelNotFoundException();

        Room room = new Room() {
            HotelId = hotel.HotelId,
            Name = roomInsert.Name,
            Capacity = roomInsert.Capacity,
            Image = roomInsert.Image,
        };

        _context.Rooms.Add(room);
        _context.SaveChanges();

        RoomDto newRoom = new RoomDto {
            RoomId = room.RoomId,
            Name = room.Name,
            Capacity = room.Capacity,
            Image = room.Image,
            Hotel = GetHotelById(room.HotelId),
        };
        return newRoom;
    }

    public void DeleteRoom(int RoomId) {
        _context.Rooms.Remove(_context.Rooms.First(r => r.RoomId == RoomId));
        _context.SaveChanges();
    }
}