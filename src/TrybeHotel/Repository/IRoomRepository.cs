using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IRoomRepository {
    IEnumerable<RoomDto> GetRooms(int HotelId);
    RoomDto AddRoom(RoomInsertDto roomInsert);

    void DeleteRoom(int RoomId);
}