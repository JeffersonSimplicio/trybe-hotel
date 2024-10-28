using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Utils;
using TrybeHotel.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace TrybeHotel.Repository;

public class HotelRepository : IHotelRepository {
    protected readonly ITrybeHotelContext _context;
    protected readonly IGetModel _getModel;
    public HotelRepository(ITrybeHotelContext context, IGetModel getModel) {
        _context = context;
        _getModel = getModel;
    }

    public HotelDto GetHotelById(int hotelId) {
        HotelDto? selectedHotel = (from hotel in _context.Hotels
                                   join city in _context.Cities
                                   on hotel.CityId equals city.CityId
                                   where hotel.HotelId == hotelId
                                   select new HotelDto {
                                       HotelId = hotel.HotelId,
                                       Name = hotel.Name,
                                       Address = hotel.Address,
                                       CityId = city.CityId,
                                       cityName = city.Name,
                                       state = city.State,
                                   }).FirstOrDefault();
        if (selectedHotel == null) throw new HotelNotFoundException();
        return selectedHotel;
    }

    public IEnumerable<HotelDto> GetHotelsByName(string hotelName) {
        var hotels = from hotel in _context.Hotels
                     join city in _context.Cities
                     on hotel.CityId equals city.CityId
                     where hotel.Name.ToLower().Contains(hotelName.ToLower())
                     select new HotelDto {
                         HotelId = hotel.HotelId,
                         Name = hotel.Name,
                         Address = hotel.Address,
                         CityId = city.CityId,
                         cityName = city.Name,
                         state = city.State,
                     };
        return hotels;
    }

    public IEnumerable<HotelDto> GetAllHotels() {
        var hotels = from hotel in _context.Hotels
                     join city in _context.Cities
                     on hotel.CityId equals city.CityId
                     select new HotelDto {
                         HotelId = hotel.HotelId,
                         Name = hotel.Name,
                         Address = hotel.Address,
                         CityId = city.CityId,
                         cityName = city.Name,
                         state = city.State,
                     };
        return hotels;
    }

    public HotelDto AddHotel(HotelInsertDto hotelInsert) {
        City city = _getModel.City(hotelInsert.cityId);

        bool hotelExists = _context.Hotels.Any(
            h =>
                h.Name.ToLower() == hotelInsert.name.ToLower() &&
                h.CityId == hotelInsert.cityId
        );
        if (hotelExists) throw new HotelAlreadyExistsException(hotelInsert.name, city.Name);

        Hotel hotel = SimpleMapper.Map<HotelInsertDto, Hotel>(hotelInsert);

        _context.Hotels.Add(hotel);
        _context.SaveChanges();

        return new HotelDto {
            HotelId = hotel.HotelId,
            Name = hotel.Name,
            Address = hotel.Address,
            CityId = hotel.CityId,
            cityName = city.Name,
            state = city.State,
        };
    }

    public HotelDto UpdateHotel(int hotelId, HotelInsertDto hotelInsert) {
        // Verifica se o hotel existe
        Hotel? hotelExists = _context.Hotels.SingleOrDefault(h => h.HotelId == hotelId);
        if (hotelExists == null) throw new HotelNotFoundException();

        // Verifica se o cityId passado pertece a alguma cidade
        City hotelCity = _getModel.City(hotelInsert.cityId);

        // Verifica se existe outro hotel na mesma cidade com o mesmo nome
        // Além do que esta sendo editado.
        bool duplicateHotelExists = _context.Hotels.Any(
            h =>
                h.Name.ToLower() == hotelInsert.name.ToLower() &&
                h.CityId == hotelInsert.cityId &&
                h.HotelId != hotelId
        );
        if (duplicateHotelExists) throw new HotelAlreadyExistsException();

        hotelExists.Name = hotelInsert.name;
        hotelExists.Address = hotelInsert.address;
        hotelExists.CityId = hotelInsert.cityId;

        _context.SaveChanges();

        return new HotelDto {
            HotelId = hotelExists.HotelId,
            Name = hotelExists.Name,
            Address = hotelExists.Address,
            CityId = hotelExists.CityId,
            cityName = hotelCity.Name,
            state = hotelCity.State,
        };
    }

    public void DeleteHotel(int hotelId) {
        Hotel? selectedHotel = _context
            .Hotels
            .Where(h => h.HotelId == hotelId)
            .FirstOrDefault();
        if (selectedHotel == null) throw new HotelNotFoundException();
        _context.Hotels.Remove(selectedHotel);
        _context.SaveChanges();
    }
}