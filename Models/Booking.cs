namespace BookingApp.Api.Models;

public class Booking
{
    public int Id { get; set; }

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; } // exclusive

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public BookingStatus Status {get; set;} = BookingStatus.Pending;
}