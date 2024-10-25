using TrybeHotel.Models;

namespace TrybeHotel.Utils;

public interface IGetModel {
    User? UserOrDefault(int userId);
    User? UserOrDefault(string userEmail);
    User User(int userId);
    User User(string userEmail);
    City? CityOrDefault(int cityId);
    City City(int cityId);
    Room? RoomOrDefault(int roomId);
    Room Room(int roomId);
}
