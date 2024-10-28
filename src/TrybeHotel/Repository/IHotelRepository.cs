using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IHotelRepository {
    IEnumerable<HotelDto> GetHotelsByCity(int cityId);
    IEnumerable<HotelDto> GetAllHotels();
    HotelDto AddHotel(HotelInsertDto hotelInsert);
}