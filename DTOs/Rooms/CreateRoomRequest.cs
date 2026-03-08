namespace BookingApp.Api.DTOs.Rooms;

public record CreateRoomRequest(
    string Name,
    string? Description,
    decimal PricePerNight,
    int Capacity
);