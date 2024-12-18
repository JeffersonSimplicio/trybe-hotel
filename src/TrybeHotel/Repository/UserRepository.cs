using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;
using TrybeHotel.Utils;
using System.Linq;

namespace TrybeHotel.Repository;

public class UserRepository : IUserRepository {
    protected readonly ITrybeHotelContext _context;
    protected readonly IGetModel _getModel;

    public UserRepository(ITrybeHotelContext context, IGetModel getModel) {
        _context = context;
        _getModel = getModel;
    }

    public UserDto Login(LoginDto login) {
        User user = _getModel.User(login.Email);

        if (user.Password != login.Password) throw new UnauthorizedAccessException();

        return SimpleMapper.Map<User, UserDto>(user);
    }

    public UserDto AddUser(UserCreateDto user) {
        try {
            GetUserByEmail(user.Email);
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

    public UserDto GetUserById(int userId) {
        User user = _getModel.User(userId);
        return SimpleMapper.Map<User, UserDto>(user);
    }

    public UserDto GetUserByEmail(string userEmail) {
        User user = _getModel.User(userEmail);
        return SimpleMapper.Map<User, UserDto>(user);
    }

    public IEnumerable<UserDto> GetUsersByName(string userName) {
        return _context.Users
            .Where(u => u.Name.ToLower().Contains(userName.ToLower()))
            .Select(u => SimpleMapper.Map<User, UserDto>(u));
    }

    public IEnumerable<UserDto> GetAllUsers() {
        return _context.Users.Select(u => SimpleMapper.Map<User, UserDto>(u));
    }

    public UserDto UpdateUser(int userId, UserUpdateDto userUpdate, string userType) {
        User user = _getModel.User(userId);

        bool duplicateUserExists = _context.Users.Any(
            u => u.Email.ToLower() == userUpdate.Email.ToLower() &&
            u.UserId != userId
        );
        if (duplicateUserExists) throw new EmailAlreadyExistsException(userUpdate.Email);

        user.Name = userUpdate.Name;
        user.Email = userUpdate.Email;
        // Caso o usuario informe a senha, ela sera atualizada
        if (userUpdate.Password != null) user.Password = userUpdate.Password;
        // Apenas "Admin", podem atualizar o userType do usuario.
        if (userType == "admin") user.UserType = userUpdate.UserType;

        _context.SaveChanges();

        return SimpleMapper.Map<User, UserDto>(user);
    }

    public void DeleteOwnAccount(string userEmail) {
        User user = _getModel.User(userEmail);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public void AdminDeleteUser(int userId) {
        User user = _getModel.User(userId);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}