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
        return _context.Users.FirstOrDefault(u => u.UserId == userId);
    }

    public User? UserOrDefault(string userEmail) {
        return _context.Users.FirstOrDefault(u => u.Email == userEmail);
    }

    public User User(int userId) {
        User? user = UserOrDefault(userId);
        if (user == null) throw new UserNotFoundException();
        return user;
    }

    public User User(string userEmail) {
        User? user = UserOrDefault(userEmail);
        if (user == null) throw new UserNotFoundException();
        return user;
    }

    public Room? RoomOrDefault(int roomId) {
        return _context.Rooms.FirstOrDefault(r => r.RoomId == roomId);
    }

    public Room Room(int roomId) {
        Room? room = RoomOrDefault(roomId);
        if (room == null) throw new RoomNotFoundException();
        return room;
    }
}
