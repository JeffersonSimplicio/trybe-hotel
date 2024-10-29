using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Utils;
using TrybeHotel.Exceptions;

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

    public IEnumerable<RoomDto> GetRooms(int HotelId) {
        HotelDto hotel = GetHotelById(HotelId);

        var rooms = _context.Rooms
            .Where(r => r.HotelId == HotelId)
            .Select(r => new RoomDto {
                roomId = r.RoomId,
                name = r.Name,
                capacity = r.Capacity,
                image = r.Image,
                hotel = hotel,
            });
        return rooms;
    }

    public RoomDto AddRoom(RoomInsertDto roomInsert) {
        Hotel? hotel = _context.Hotels.SingleOrDefault(h => h.HotelId == roomInsert.HotelId);
        if (hotel == null) throw new HotelNotFoundException();

        Room room = new Room() {
            HotelId= hotel.HotelId,
            Name = roomInsert.Name,
            Capacity = roomInsert.Capacity,
            Image = roomInsert.Image,
        };

        _context.Rooms.Add(room);
        _context.SaveChanges();

        RoomDto newRoom = new RoomDto {
            roomId = room.RoomId,
            name = room.Name,
            capacity = room.Capacity,
            image = room.Image,
            hotel = GetHotelById(room.HotelId),
        };
        return newRoom;
    }

    public void DeleteRoom(int RoomId) {
        _context.Rooms.Remove(_context.Rooms.First(r => r.RoomId == RoomId));
        _context.SaveChanges();
    }
}