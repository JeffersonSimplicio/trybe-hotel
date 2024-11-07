namespace TrybeHotel.Dto;

public class CityBaseDto {
    public string Name { get; set; }
    public string State { get; set; }
}

public class CityDto : CityBaseDto {
    public int CityId { get; set; }
}

public class CityCreateDto : CityBaseDto { }

public class CityUpdateDto : CityBaseDto { }