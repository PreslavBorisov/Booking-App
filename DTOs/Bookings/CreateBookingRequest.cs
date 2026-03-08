namespace BookingApp.Api.DTOs.Bookings;

public record CreateBookingRequest(int RoomId, DateOnly CheckIn, DateOnly CheckOut);