using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;
using TrybeHotel.Utils;

namespace TrybeHotel.Repository;
public class CityRepository : ICityRepository {
    protected readonly ITrybeHotelContext _context;
    public CityRepository(ITrybeHotelContext context) {
        _context = context;
    }

    // 4. Refatore o endpoint GET /city
    public IEnumerable<CityDto> GetCities() {
        var cities = _context.Cities.Select(c => SimpleMapper.Map<City, CityDto>(c));
        return cities;
    }

    // 2. Refatore o endpoint POST /city
    public CityDto AddCity(City city) {
        _context.Cities.Add(city);
        _context.SaveChanges();
        return SimpleMapper.Map<City, CityDto>(city);
    }

    // 3. Desenvolva o endpoint PUT /city
    public CityDto UpdateCity(City city) {
        _context.Cities.Update(city);
        _context.SaveChanges();
        return SimpleMapper.Map<City, CityDto>(city);
    }
}