using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IUserRepository {
    UserDto Add(UserDtoInsert user);
    UserDto Login(LoginDto login);
    UserDto GetUser(int userId);
    UserDto GetUser(string userEmail);
    IEnumerable<UserDto> GetUsers();
    UserDto Update(UserDtoUpdate userUpdate, string userType);
    void Delete(int userId);

}