namespace BookingApp.Api.Models;

public class Room
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;        
    public string? Description { get; set; }

    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; }               

    public bool IsActive { get; set; } = true;

    public string? ImageUrl { get; set;}

    public string? Location { get; set;}

    public string? Address { get; set;}

    public List<String> Amenities { get; set;} = new();

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}