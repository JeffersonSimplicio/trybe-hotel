using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IUserRepository {
    UserDto AddUser(UserDtoInsert user);
    UserDto Login(LoginDto login);
    UserDto GetUserById(int userId);
    UserDto GetUserByEmail(string userEmail);
    IEnumerable<UserDto> GetUsersByName(string userName);
    IEnumerable<UserDto> GetAllUsers();
    UserDto UpdateUser(int userId, UserDtoUpdate userUpdate, string userType);
    void DeleteOwnAccount(string userEmail);
    void AdminDeleteUser(int userId);
}