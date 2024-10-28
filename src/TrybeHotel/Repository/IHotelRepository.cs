using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IHotelRepository {
    IEnumerable<HotelDto> GetHotelsByCity(int cityId);
    IEnumerable<HotelDto> GetHotelsByName(string hotelName);
    IEnumerable<HotelDto> GetAllHotels();
    HotelDto AddHotel(HotelInsertDto hotelInsert);
}