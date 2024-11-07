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

    public HotelWithRoomsDto GetHotelById(int hotelId) {
        HotelWithRoomsDto? hotelData = _context
            .Hotels
            .Include(h => h.City)
            .Include(h => h.Rooms)
            .Where(h => h.HotelId == hotelId)
            .AsEnumerable()
            .Select(h => new HotelWithRoomsDto {
                HotelId = h.HotelId,
                Name = h.Name,
                Address = h.Address,
                City = new CityDto {
                    CityId = h.CityId,
                    Name = h.City.Name,
                    State = h.City.State,
                },
                Rooms = (h.Rooms ?? new List<Room>()).Select(r => new RoomDto {
                    RoomId = r.RoomId,
                    Name = r.Name,
                    Capacity = r.Capacity,
                    Image = r.Image,
                }).ToList()
            }).SingleOrDefault();

        if (hotelData == null) throw new HotelNotFoundException();
        return hotelData;
    }

    public IEnumerable<HotelDto> GetHotelsByName(string hotelName) {
        var hotels = from hotel in _context.Hotels
                     where hotel.Name.ToLower().Contains(hotelName.ToLower())
                     select new HotelDto {
                         HotelId = hotel.HotelId,
                         Name = hotel.Name,
                         Address = hotel.Address,
                         CityId = hotel.CityId,
                     };
        return hotels;
    }

    public IEnumerable<HotelDto> GetAllHotels() {
        var hotels = from hotel in _context.Hotels
                     select new HotelDto {
                         HotelId = hotel.HotelId,
                         Name = hotel.Name,
                         Address = hotel.Address,
                         CityId = hotel.CityId,
                     };
        return hotels;
    }

    public HotelDto AddHotel(HotelCreateDto newHotel) {
        City city = _getModel.City(newHotel.CityId);

        bool hotelExists = _context.Hotels.Any(
            h =>
                h.Name.ToLower() == newHotel.Name.ToLower() &&
                h.CityId == newHotel.CityId
        );
        if (hotelExists) throw new HotelAlreadyExistsException(newHotel.Name, city.Name);

        Hotel hotel = SimpleMapper.Map<HotelCreateDto, Hotel>(newHotel);

        _context.Hotels.Add(hotel);
        _context.SaveChanges();

        return new HotelDto {
            HotelId = hotel.HotelId,
            Name = hotel.Name,
            Address = hotel.Address,
            CityId = hotel.CityId
        };
    }

    public HotelDto UpdateHotel(int hotelId, HotelUpdateDto updateHotel) {
        // Verifica se o hotel existe
        Hotel? hotelExists = _context.Hotels.SingleOrDefault(h => h.HotelId == hotelId);
        if (hotelExists == null) throw new HotelNotFoundException();

        // Verifica se o cityId passado pertece a alguma cidade
        _getModel.City(updateHotel.CityId);

        // Verifica se existe outro hotel na mesma cidade com o mesmo nome
        // Além do que esta sendo editado.
        bool duplicateHotelExists = _context.Hotels.Any(
            h =>
                h.Name.ToLower() == updateHotel.Name.ToLower() &&
                h.CityId == updateHotel.CityId &&
                h.HotelId != hotelId
        );
        if (duplicateHotelExists) throw new HotelAlreadyExistsException();

        hotelExists.Name = updateHotel.Name;
        hotelExists.Address = updateHotel.Address;
        hotelExists.CityId = updateHotel.CityId;

        _context.SaveChanges();

        return new HotelDto {
            HotelId = hotelExists.HotelId,
            Name = hotelExists.Name,
            Address = hotelExists.Address,
            CityId = hotelExists.CityId,
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