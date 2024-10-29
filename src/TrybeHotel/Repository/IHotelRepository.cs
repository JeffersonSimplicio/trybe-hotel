using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IHotelRepository {
    HotelWithRoomsDto GetHotelById(int hotelId);
    IEnumerable<HotelDto> GetAllHotels();
    IEnumerable<HotelDto> GetHotelsByName(string hotelName);
    HotelDto AddHotel(HotelInsertDto hotelInsert);
    HotelDto UpdateHotel(int hotelId, HotelInsertDto hotelInsert);
    void DeleteHotel(int hotelId);
}