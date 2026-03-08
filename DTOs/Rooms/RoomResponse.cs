namespace BookingApp.Api.DTOs.Rooms;

public record RoomResponse(
    int Id,
    string Name,
    string? Description,
    decimal PricePerNight,
    int Capacity,
    bool IsActive
);