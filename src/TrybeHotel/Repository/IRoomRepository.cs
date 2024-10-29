using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IRoomRepository {
    RoomDto GetRoomById(int roomId);
    IEnumerable<RoomInfoDto> GetAllRooms(int page, int size);
    RoomDto AddRoom(RoomInsertDto roomInsert);
    RoomDto UpdateRoom(int roomId, RoomInsertDto roomInsert);
    void DeleteRoom(int RoomId);
}