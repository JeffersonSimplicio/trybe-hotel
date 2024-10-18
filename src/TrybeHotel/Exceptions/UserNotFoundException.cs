namespace TrybeHotel.Exceptions;

public class UserNotFoundException : NotFoundException {
    public UserNotFoundException()
        : base("User not found.") { }
    public UserNotFoundException(string userName)
        : base($"The user, {userName}, was not found.") { }
}
