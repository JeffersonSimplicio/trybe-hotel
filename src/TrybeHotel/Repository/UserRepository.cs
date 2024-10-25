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

    public UserDto Login(LoginDto login) {
        User user = _getModel.User(login.Email);

        if (user.Password != login.Password) throw new UnauthorizedAccessException();

        return SimpleMapper.Map<User, UserDto>(user);
    }

    public UserDto Add(UserDtoInsert user) {
        try {
            Get(user.Email);
            //Exception thrown if the email is already registered.
            throw new EmailAlreadyExistsException(user.Email);
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

    public UserDto Get(int userId) {
        User user = _getModel.User(userId);
        return SimpleMapper.Map<User, UserDto>(user);
    }

    public UserDto Get(string userEmail) {
        User user = _getModel.User(userEmail);
        return SimpleMapper.Map<User, UserDto>(user);
    }

    public IEnumerable<UserDto> GetAll() {
        return _context.Users.Select(u => SimpleMapper.Map<User, UserDto>(u));
    }

    public UserDto Update(UserDtoUpdate userUpdate, string userType) {
        User user = _getModel.User(userUpdate.UserId);

        user.Name = userUpdate.Name;
        user.Email = userUpdate.Email;
        if (userUpdate.Password != null) user.Password = userUpdate.Password;
        // Apenas "Admin", podem atualizar o userType do usuario.
        if (userType == "admin") user.UserType = userUpdate.UserType;

        _context.SaveChanges();

        return SimpleMapper.Map<User, UserDto>(user);
    }

    public void Delete(int userId) {
        throw new NotImplementedException();
    }
}