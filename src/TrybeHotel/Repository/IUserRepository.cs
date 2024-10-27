using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IUserRepository {
    UserDto AddUser(UserDtoInsert user);
    UserDto Login(LoginDto login);
    UserDto GetUserById(int userId);
    UserDto GetUserByEmail(string userEmail);
    IEnumerable<UserDto> GetAllUsers();
    UserDto UpdateUser(UserDtoUpdate userUpdate, string userType);
    void DeleteUser(int userId);
}