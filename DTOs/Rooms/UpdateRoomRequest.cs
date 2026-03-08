namespace BookingApp.Api.DTOs.Rooms;
public record UpdateRoomRequest(
    string Name,
    string? Description,
    decimal PricePerNight,
    int Capacity,
    bool IsActive
);