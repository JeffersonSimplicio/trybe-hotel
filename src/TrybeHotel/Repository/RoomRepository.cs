using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public class RoomRepository : IRoomRepository {
    protected readonly ITrybeHotelContext _context;
    public RoomRepository(ITrybeHotelContext context) {
        _context = context;
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
                                      cityName = city.Name,
                                      state = city.State,
                                  }).First();
        return selectedHotel;
    }

    // 7. Refatore o endpoint GET /room
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

    // 8. Refatore o endpoint POST /room
    public RoomDto AddRoom(Room room) {
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