namespace TrybeHotel.Dto;

public class HotelDto {
    public int? HotelId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int CityId { get; set; }
    public string? cityName { get; set; }
    public string state { get; set; }
}

public class HotelInsertDto {
    public string name { get; set; }
    public string address { get; set; }
    public int cityId { get; set; }
}