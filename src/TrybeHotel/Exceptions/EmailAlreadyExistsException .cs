namespace TrybeHotel.Exceptions;

public class EmailAlreadyExistsException : Exception {
    public EmailAlreadyExistsException()
        : base("User email already exists") { }
    public EmailAlreadyExistsException(string email)
        : base($"The email '{email}' is already registered.") { }
}