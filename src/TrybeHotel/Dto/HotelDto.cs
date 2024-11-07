namespace TrybeHotel.Dto;

public class HotelBaseDto {
    public string Name { get; set; }
    public string Address { get; set; }
}

public class HotelDto : HotelBaseDto {
    public int HotelId { get; set; }
    public int CityId { get; set; }
}

public class HotelWithCityDto : HotelBaseDto {
    public int HotelId { get; set; }
    public CityDto City { get; set; }
}

public class HotelCreateDto : HotelBaseDto {
    public int CityId { get; set; }
}

public class HotelUpdateDto : HotelBaseDto {
    public int CityId { get; set; }
}

public class HotelWithRoomsDto : HotelWithCityDto {
    public IEnumerable<RoomDto> Rooms { get; set; }
}