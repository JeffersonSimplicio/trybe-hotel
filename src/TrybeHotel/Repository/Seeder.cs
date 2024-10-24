using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository;

public static class Seeder {
    public static void SeedUsers(ITrybeHotelContext _context) {
        try {
            int usersCount = _context.Users.Count();
            if (usersCount == 0) {
                var users = new List<User>() {
                    new User {
                        Name = "admin",
                        Email = "admin@admin.com",
                        Password = "admin",
                        UserType = "admin"
                    },
                    new User {
                        Name = "client",
                        Email = "client@client.com",
                        Password = "client",
                        UserType = "client"
                    }
                };
                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
        }
        catch (Exception ex) {
            Console.WriteLine(ex.ToString());
        }
    }
} 