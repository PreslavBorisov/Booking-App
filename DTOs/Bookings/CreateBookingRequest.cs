using System.ComponentModel.DataAnnotations;

namespace BookingApp.Api.DTOs.Bookings;

public record CreateBookingRequest(
    [Range(1, int.MaxValue)] 
    int RoomId,
    [Required(ErrorMessage = "Check-in date is required")]
    DateOnly CheckIn,
    [Required(ErrorMessage = "Check-out date is required")]
    DateOnly CheckOut
);