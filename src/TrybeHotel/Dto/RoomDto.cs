namespace TrybeHotel.Dto;

public class RoomDto {
    public int roomId { get; set; }
    public string name { get; set; }
    public int capacity { get; set; }
    public string image { get; set; }
    public HotelDto? hotel { get; set; }
}

public class RoomInsertDto {
    public string Name { get; set; }
    public int Capacity { get; set; }
    public string Image { get; set; }
    public int HotelId { get; set; }
}

public class HotelRoomDto {
    public int RoomId { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public string Image { get; set; }
}