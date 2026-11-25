namespace BookingApp.Api.DTOs.Bookings;

public record AdminBookingResponse(
    int Id,
    int RoomId,
    string RoomName,
    int UserId,
    string UserName,
    DateOnly CheckIn,
    DateOnly CheckOut,
    string Status,
    string? ImageUrl
);