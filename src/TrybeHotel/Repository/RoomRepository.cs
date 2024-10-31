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
                                      CityName = city.Name,
                                      State = city.State,
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
            Hotel = GetHotelDtoById(room.HotelId),
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
                HotelId = r.HotelId,
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
            Hotel = GetHotelDtoById(room.HotelId),
        };
        return newRoom;
    }

    public RoomDto UpdateRoom(int roomId, RoomInsertDto roomInsert) {
        // VerificationException se o quarto existe
        Room room = _getModel.Room(roomId);

        // Verifica se o hotel existe
        HotelDto hotel = GetHotelDtoById(roomInsert.HotelId);

        room.HotelId = roomInsert.HotelId;
        room.Name = roomInsert.Name;
        room.Capacity = roomInsert.Capacity;
        room.Image = roomInsert.Image;

        _context.SaveChanges();

        return new RoomDto {
            RoomId=room.RoomId,
            Name = roomInsert.Name,
            Capacity=roomInsert.Capacity,
            Image = roomInsert.Image,
            Hotel = hotel,
        };
    }

    public void DeleteRoom(int roomId) {
        Room room = _getModel.Room(roomId);
        _context.Rooms.Remove(room);
        _context.SaveChanges();
    }
}