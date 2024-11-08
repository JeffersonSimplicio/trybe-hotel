namespace TrybeHotel.Dto;

public class HotelBaseDto {
    public string Name { get; set; }
    public string Address { get; set; }
    public int CityId { get; set; }
}

public class HotelDto : HotelBaseDto {
    public int HotelId { get; set; }
}

public class HotelCreateDto : HotelBaseDto { }

public class HotelUpdateDto : HotelBaseDto { }

public class HotelReferenceDto {
    public int HotelId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public IEnumerable<RoomReferenceDto>? Rooms { get; set; }
}