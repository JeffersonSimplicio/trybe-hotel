namespace TrybeHotel.Dto;

public class RoomBaseDto {
    public string Name { get; set; }
    public int Capacity { get; set; }
    public string Image { get; set; }
    public int HotelId { get; set; }
}

public class RoomDto : RoomBaseDto {
    public int RoomId { get; set; }
}

public class RoomCreateDto : RoomBaseDto { }

public class RoomUpdateDto : RoomBaseDto { }

public class RoomReferenceDto {
    public int RoomId { get; set; }
    public int Name { get; set; }
    public int Capacity { get; set; }
    public string Image { get; set; }
    public IEnumerable<BookingReferenceDto>? Bookings { get; set; }
}