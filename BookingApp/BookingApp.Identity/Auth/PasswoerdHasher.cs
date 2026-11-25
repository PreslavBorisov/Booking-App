using BookingApp.Api.Auth.Interfaces;

namespace BookingApp.Api.Auth;

// За учебен проект е ок. (PBKDF2 via built-in)
public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.Verify(password, passwordHash);
}