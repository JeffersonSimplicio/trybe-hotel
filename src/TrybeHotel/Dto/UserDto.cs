namespace TrybeHotel.Dto;

public class UserDto {
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserType { get; set; }
}

public class UserCreateDto {
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserUpdateDto {
    //Futuramente tornar tudo opcional,
    //assim cada coisa pode ser atualizada de forma independente.
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserType { get; set; }
    public string? Password { get; set; }
}

public class LoginDto {
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserReferenceDto {
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}