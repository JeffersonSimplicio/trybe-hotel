using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IHotelRepository {
    IEnumerable<HotelDto> GetHotelsByName(string hotelName);
    IEnumerable<HotelDto> GetAllHotels();
    HotelDto AddHotel(HotelInsertDto hotelInsert);
}