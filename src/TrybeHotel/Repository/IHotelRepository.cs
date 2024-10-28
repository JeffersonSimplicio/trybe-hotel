using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IHotelRepository {
    HotelDto GetHotelById(int hotelId);
    IEnumerable<HotelDto> GetAllHotels();
    IEnumerable<HotelDto> GetHotelsByName(string hotelName);
    HotelDto AddHotel(HotelInsertDto hotelInsert);
    void DeleteHotel(int hotelId);
}