using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IRoomRepository {
    RoomDto GetRoomById(int roomId);
    IEnumerable<RoomDto> GetAllRooms(int page, int size);
    //Implementar FindRoomsByName e GetAllBookingsByRoom
    RoomDto AddRoom(RoomCreateDto newRoom);
    RoomDto UpdateRoom(int roomId, RoomUpdateDto updateRoom);
    void DeleteRoom(int roomId);
}