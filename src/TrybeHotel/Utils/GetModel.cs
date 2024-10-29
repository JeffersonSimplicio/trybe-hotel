using TrybeHotel.Models;
using TrybeHotel.Exceptions;
using TrybeHotel.Repository;

namespace TrybeHotel.Utils;

public class GetModel : IGetModel {
    protected readonly ITrybeHotelContext _context;

    public GetModel(ITrybeHotelContext context) {
        _context = context;
    }

    public User? UserOrDefault(int userId) {
        return _context.Users.SingleOrDefault(u => u.UserId == userId);
    }

    public User? UserOrDefault(string userEmail) {
        return _context.Users.SingleOrDefault(u => u.Email == userEmail);
    }

    public User User(int userId) {
        User? user = UserOrDefault(userId);
        return user ?? throw new UserNotFoundException();
    }

    public User User(string userEmail) {
        User? user = UserOrDefault(userEmail);
        return user ?? throw new UserNotFoundException();
    }

    public City? CityOrDefault(int cityId) {
        return _context.Cities.SingleOrDefault(c => c.CityId == cityId);
    }

    public City City(int cityId) {
        City? city = CityOrDefault(cityId);
        return city ?? throw new CityNotFoundException();
    }

    public Room? RoomOrDefault(int roomId) {
        return _context.Rooms.SingleOrDefault(r => r.RoomId == roomId);
    }

    public Room Room(int roomId) {
        Room? room = RoomOrDefault(roomId);
        return room ?? throw new RoomNotFoundException();
    }
}
