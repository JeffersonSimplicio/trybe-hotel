using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IHotelRepository {
    HotelWithRoomsDto GetHotelById(int hotelId);
    IEnumerable<HotelDto> GetAllHotels();
    IEnumerable<HotelDto> GetHotelsByName(string hotelName);
    HotelDto AddHotel(HotelCreateDto newHotel);
    HotelDto UpdateHotel(int hotelId, HotelUpdateDto updateHotel);
    void DeleteHotel(int hotelId);
}