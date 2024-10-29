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

    public CityDto GetCityById(int id) {
        City city = _getModel.City(id);
        return SimpleMapper.Map<City, CityDto>(city);
    }

    public IEnumerable<CityDto> GetAllCities() {
        var cities = _context.Cities.Select(c => SimpleMapper.Map<City, CityDto>(c));
        return cities;
    }

    public IEnumerable<CityDto> FindCitiesByName(string name) {
        var cities = _context.Cities
            .Where(c => c.Name.ToLower().Contains(name.ToLower()))
            .Select(c => SimpleMapper.Map<City, CityDto>(c));
        return cities;
    }

    public IEnumerable<HotelDto> FindHotelsByCity(int cityId) {
        _getModel.City(cityId);
        var hotels = from hotel in _context.Hotels
                     join city in _context.Cities
                     on hotel.CityId equals city.CityId
                     where hotel.CityId == cityId
                     select new HotelDto {
                         HotelId = hotel.HotelId,
                         Name = hotel.Name,
                         Address = hotel.Address,
                         CityId = city.CityId,
                         CityName = city.Name,
                         State = city.State,
                     };
        return hotels;
    }

    public CityDto AddCity(CityDtoInsert city) {
        bool CityExists = _context
            .Cities
            .Any(c =>
                c.Name.ToLower() == city.Name.ToLower() &&
                c.State.ToLower() == city.State.ToLower()
            );
        if (CityExists) throw new CityAlreadyExistsException(city.Name, city.State);

        City cityEntity = SimpleMapper.Map<CityDtoInsert, City>(city);

        _context.Cities.Add(cityEntity);
        _context.SaveChanges();
        return SimpleMapper.Map<City, CityDto>(cityEntity);
    }

    public CityDto UpdateCity(int cityId, CityDtoInsert city) {
        City existingCity = _getModel.City(cityId);

        bool duplicateCityExists = _context.Cities.Any(
            c => c.Name.ToLower() == city.Name.ToLower() &&
            c.State.ToLower() == city.State.ToLower() &&
            c.CityId != cityId
        );
        if (duplicateCityExists) throw new CityAlreadyExistsException(city.Name, city.State);

        existingCity.Name = city.Name;
        existingCity.State = city.State;

        _context.SaveChanges();
        return SimpleMapper.Map<City, CityDto>(existingCity);
    }

    public void DeleteCity(int id) {
        City city = _getModel.City(id);
        _context.Cities.Remove(city);
        _context.SaveChanges();
    }
}