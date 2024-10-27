using TrybeHotel.Models;

namespace TrybeHotel.Exceptions;

public class CityAlreadyExistsException : EntityAlreadyExistsException {
    public CityAlreadyExistsException()
        : base("This city already exists and duplicate information of this data is not allowed.") { }

    public CityAlreadyExistsException(string name, string state)
        : base($"City with name '{name}' in state '{state}' already exists.") {
    }
}