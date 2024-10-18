using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;
using TrybeHotel.Utils;

namespace TrybeHotel.Repository;

public class UserRepository : IUserRepository {
    protected readonly ITrybeHotelContext _context;
    protected readonly IGetModel _getModel;

    public UserRepository(
        ITrybeHotelContext context,
        IGetModel getModel
    ) {
        _context = context;
        _getModel = getModel;
    }

    public UserDto GetUserById(int userId) {
        User user = _getModel.User(userId);
        return SimpleMapper.Map<User, UserDto>(user);
    }

    public UserDto Login(LoginDto login) {
        User user = _getModel.User(login.Email);

        if (user.Password != login.Password) throw new UnauthorizedAccessException();

        return SimpleMapper.Map<User, UserDto>(user);
    }

    public UserDto Add(UserDtoInsert user) {
        try {
            GetUserByEmail(user.Email);
            //Exception thrown if the email is already registered.
            throw new EmailAlreadyExistsException();
        }
        catch (UserNotFoundException) {
            User newUser = new User() {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client"
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return SimpleMapper.Map<User, UserDto>(newUser);
        }
    }

    public UserDto GetUserByEmail(string userEmail) {
        User user = _getModel.User(userEmail);
        return SimpleMapper.Map<User, UserDto>(user);
    }

    public IEnumerable<UserDto> GetUsers() {
        return _context.Users.Select(u => SimpleMapper.Map<User, UserDto>(u));
    }
}