using System.ComponentModel.DataAnnotations;

namespace BookingApp.Api.DTOs.Rooms;

public record CreateRoomRequest(
    [property: Required, MaxLength(120)] string Name,
    [property: MaxLength(2000)] string? Description,
    [property: Range(0.01, 1000000)] decimal PricePerNight,
    [property: Range(1, 100)] int Capacity,
    [property: MaxLength(1000)] string? ImageUrl,
    [property: MaxLength(200)] string? Location,
    [property: MaxLength(300)] string? Address,
    List<string>? Amenities
);