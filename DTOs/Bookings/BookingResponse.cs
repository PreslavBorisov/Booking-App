namespace BookingApp.Api.DTOs.Bookings;

public record BookingResponse(int Id, int RoomId,string RoomName, int UserId, DateOnly CheckIn, DateOnly CheckOut, string Status, string? ImageUrl);