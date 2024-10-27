using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface ICityRepository {
    CityDto GetCityById(int id);
    IEnumerable<CityDto> GetAllCities();
    IEnumerable<CityDto> FindCitiesByName(string name);
    CityDto AddCity(CityDtoInsert city);
    //Talvez depois deva cria uma DTO para update
    CityDto UpdateCity(CityDto city);
    void DeleteCity(int id);
}