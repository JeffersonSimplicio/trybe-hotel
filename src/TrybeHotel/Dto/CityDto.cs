namespace TrybeHotel.Dto;

public class CityDto {
    public int cityId { get; set; }
    public string name { get; set; }
    public string state { get; set; }
}

public class CityDtoInsert {
    public string name { get; set; }
    public string state { get; set; }
}