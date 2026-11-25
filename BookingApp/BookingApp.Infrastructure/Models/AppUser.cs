namespace BookingApp.Api.Models;

public class AppUser
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = "User";

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}