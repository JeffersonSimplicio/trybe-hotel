namespace TrybeHotel.Exceptions;

public class RoomNotFoundException : NotFoundException {
    public RoomNotFoundException()
        : base("Room not found.") { }
    public RoomNotFoundException(string roomName)
        : base($"The room, {roomName}, was not found.") { }
}
