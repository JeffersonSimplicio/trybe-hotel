using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public class HotelRepository : IHotelRepository {
    protected readonly ITrybeHotelContext _context;
    public HotelRepository(ITrybeHotelContext context) {
        _context = context;
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

    // 6. Refatore o endpoint POST /hotel
    public HotelDto AddHotel(Hotel hotel) {
        _context.Hotels.Add(hotel);
        _context.SaveChanges();
        City city = _context.Cities.First(c => c.CityId == hotel.CityId);
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