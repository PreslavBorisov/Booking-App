namespace BookingApp.Api.DTOs.Bookings;

public record BookingResponse(int Id, int RoomId, int UserId, DateOnly CheckIn, DateOnly CheckOut, string Status);