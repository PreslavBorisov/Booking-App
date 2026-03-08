namespace BookingApp.Api.DTOs.Rooms;

public record RoomAvailabilityResponse(
    int RoomId,
    DateOnly CheckIn,
    DateOnly CheckOut,
    bool IsAvailable
);