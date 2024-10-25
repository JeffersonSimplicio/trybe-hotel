namespace TrybeHotel.Exceptions;

public class EntityAlreadyExistsException : Exception {
    public EntityAlreadyExistsException()
        : base("The entity already exists and duplicate information of this data is not allowed.") { }

    public EntityAlreadyExistsException(string message)
        : base(message) { }
}
