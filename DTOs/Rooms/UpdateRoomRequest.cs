namespace BookingApp.Api.DTOs.Rooms;
public record UpdateRoomRequest(
    string Name,
    string? Description,
    decimal PricePerNight,
    int Capacity,
    bool IsActive,
    string? ImageUrl,
    string? Location,
    string? Address,
    List<string> Amenities
);