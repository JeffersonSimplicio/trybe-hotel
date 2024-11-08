namespace TrybeHotel.Dto;

public class BookingBaseDto {
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int GuestQuant { get; set; }
    public int RoomId { get; set; }
    public int UserId { get; set; }
}

public class BookingDto : BookingBaseDto {
    public int BookingId { get; set; }
}

public class BookingCreateDto : BookingBaseDto { }

public class BookingUpdateDto : BookingBaseDto { }

public class BookingReferenceDto {
    public int BookingId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int GuestQuant { get; set; }
    public UserReferenceDto? User { get; set; }
}