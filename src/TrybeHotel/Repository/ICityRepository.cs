using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface ICityRepository {
    CityDto GetById(int id);
    IEnumerable<CityDto> GetAll();
    IEnumerable<CityDto> FindByName(string name);
    CityDto Add(CityDtoInsert city);
    //Talvez depois deva cria uma DTO para update
    CityDto Update(CityDto city);
    void Delete(int id);
}