using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;
using TrybeHotel.Utils;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository;
public class CityRepository : ICityRepository {
    protected readonly ITrybeHotelContext _context;
    public CityRepository(ITrybeHotelContext context) {
        _context = context;
    }

    private City? GetById(int id) {
        return _context.Cities.FirstOrDefault(c => c.CityId == id);
    }

    public CityDto GetCity(int id) {
        City? city = GetById(id);
        return city == null
            ? throw new CityNotFoundException()
            : SimpleMapper.Map<City, CityDto>(city);
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

    public CityDto AddCity(City city) {
        var existingCity = _context
            .Cities
            .FirstOrDefault(c => c.Name == city.Name && c.State == city.State);
        if (existingCity != null) throw new CityAlreadyExistsException(city);
        _context.Cities.Add(city);
        _context.SaveChanges();
        return SimpleMapper.Map<City, CityDto>(city);
    }

    public CityDto UpdateCity(City city) {
        if (GetById(city.CityId) == null) throw new CityNotFoundException();
        _context.Cities.Update(city);
        _context.SaveChanges();
        return SimpleMapper.Map<City, CityDto>(city);
    }

    public void DeleteCity(int id) {
        City? city = GetById(id);
        if (city == null) throw new CityNotFoundException();
        _context.Cities.Remove(city);
        _context.SaveChanges();
    }
}