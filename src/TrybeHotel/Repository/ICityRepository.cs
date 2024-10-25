using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface ICityRepository {
    CityDto GetCity(int id);
    IEnumerable<CityDto> GetCities();
    IEnumerable<CityDto> GetCities(string name);
    CityDto AddCity(City city);
    CityDto UpdateCity(City city);
    void DeleteCity(int id);
}