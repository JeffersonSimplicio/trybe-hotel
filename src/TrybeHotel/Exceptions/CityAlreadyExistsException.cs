using TrybeHotel.Models;

namespace TrybeHotel.Exceptions;

public class CityAlreadyExistsException : EntityAlreadyExistsException {
    public CityAlreadyExistsException()
        : base("This city already exists and duplicate information of this data is not allowed.") { }

    public CityAlreadyExistsException(City city)
        : base($"City with name '{city.Name}' in state '{city.State}' already exists.") {
    }
}