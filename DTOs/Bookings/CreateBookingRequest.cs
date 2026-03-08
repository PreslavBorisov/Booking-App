using System.ComponentModel.DataAnnotations;

namespace BookingApp.Api.DTOs.Bookings;

public record CreateBookingRequest(
    [property: Range(1, int.MaxValue)] int RoomId,
    [property: Required] DateOnly CheckIn,
    [property: Required] DateOnly CheckOut
);