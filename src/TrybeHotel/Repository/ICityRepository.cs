using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface ICityRepository {
    CityDto GetCityById(int cityId);
    IEnumerable<CityDto> GetAllCities();
    IEnumerable<CityDto> FindCitiesByName(string cityName);
    IEnumerable<HotelDto> FindHotelsByCity(int cityId);
    CityDto AddCity(CityCreateDto newCity);
    CityDto UpdateCity(int cityId, CityUpdateDto updateCity);
    void DeleteCity(int cityId);
}