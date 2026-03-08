namespace BookingApp.Api.DTOs.Bookings;

public record AdminBookingResponse(
    int Id,
    int RoomId,
    int UserId,
    string UserName,
    DateOnly CheckIn,
    DateOnly CheckOut,
    string Status
);