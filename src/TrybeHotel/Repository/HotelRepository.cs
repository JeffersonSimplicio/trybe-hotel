using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Utils;
using TrybeHotel.Exceptions;

namespace TrybeHotel.Repository;

public class HotelRepository : IHotelRepository {
    protected readonly ITrybeHotelContext _context;
    protected readonly IGetModel _getModel;
    public HotelRepository(ITrybeHotelContext context, IGetModel getModel) {
        _context = context;
        _getModel = getModel;
    }

    //  5. Refatore o endpoint GET /hotel
    public IEnumerable<HotelDto> GetHotels() {
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
}