using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;
using TrybeHotel.Utils;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository;
public class CityRepository : ICityRepository {
    protected readonly ITrybeHotelContext _context;
    protected readonly IGetModel _getModel;

    public CityRepository(ITrybeHotelContext context, IGetModel getModel) {
        _context = context;
        _getModel = getModel;
    }

    public CityDto GetCity(int id) {
        City city = _getModel.City(id);
        return SimpleMapper.Map<City, CityDto>(city);
    }

    public IEnumerable<CityDto> GetCities() {
        var cities = _context.Cities.Select(c => SimpleMapper.Map<City, CityDto>(c));
        return cities;
    }

    public IEnumerable<CityDto> GetCities(string name) {
        var cities = _context.Cities
            .Where(c => c.Name.Contains(name))
            .Select(c => SimpleMapper.Map<City, CityDto>(c));
        return cities;
    }

    public CityDto AddCity(CityDtoInsert city) {
        var existingCity = _context
            .Cities
            .FirstOrDefault(c =>
                c.Name.ToLower() == city.name.ToLower() &&
                c.State.ToLower() == city.state.ToLower()
            );
        if (existingCity != null) throw new CityAlreadyExistsException(city.name, city.state);

        City cityEntity = SimpleMapper.Map<CityDtoInsert, City>(city);

        _context.Cities.Add(cityEntity);
        _context.SaveChanges();
        return SimpleMapper.Map<City, CityDto>(cityEntity);
    }

    public CityDto UpdateCity(CityDto city) {
        City existingCity = _getModel.City(city.cityId);

        existingCity.Name = city.name;
        existingCity.State = city.state;

        _context.SaveChanges();
        return SimpleMapper.Map<City, CityDto>(existingCity);
    }

    public void DeleteCity(int id) {
        City city = _getModel.City(id);
        _context.Cities.Remove(city);
        _context.SaveChanges();
    }
}