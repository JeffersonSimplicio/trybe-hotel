using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public interface IUserRepository {
    UserDto Add(UserDtoInsert user);
    UserDto Login(LoginDto login);
    UserDto Get(int userId);
    UserDto Get(string userEmail);
    IEnumerable<UserDto> GetAll();
    UserDto Update(UserDtoUpdate userUpdate, string userType);
    void Delete(int userId);
}