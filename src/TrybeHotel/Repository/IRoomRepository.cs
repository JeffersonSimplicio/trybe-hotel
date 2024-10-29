using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IRoomRepository {
    RoomDto GetRoomById(int roomId);
    RoomDto AddRoom(RoomInsertDto roomInsert);
    void DeleteRoom(int RoomId);
}