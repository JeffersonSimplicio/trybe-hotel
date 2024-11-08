using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IHotelRepository {
    HotelDto GetHotelById(int hotelId);
    IEnumerable<HotelDto> GetAllHotels();
    IEnumerable<HotelDto> FindHotelsByName(string hotelName);
    //Implmentar futuramente: FindRoomsByHotel
    HotelDto AddHotel(HotelCreateDto newHotel);
    HotelDto UpdateHotel(int hotelId, HotelUpdateDto updateHotel);
    void DeleteHotel(int hotelId);
}