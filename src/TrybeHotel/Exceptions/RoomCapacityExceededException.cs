namespace TrybeHotel.Exceptions;

public class RoomCapacityExceededException : Exception {
    public RoomCapacityExceededException()
        : base("Guest quantity over room capacity") { }
    public RoomCapacityExceededException(int capacity, int guestQuant)
        : base($"The room can accommodate up to {capacity} people, but {guestQuant} were reported.") { }
}
