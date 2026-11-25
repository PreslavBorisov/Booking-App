using System.ComponentModel.DataAnnotations;

namespace BookingApp.Api.DTOs.Rooms;

public record CreateRoomRequest(
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(120, ErrorMessage = "Name cannot exceed 120 characters")]
     string Name,
    [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")] string? Description,
    [Range(0.01, 1000000, ErrorMessage = "Price per night must be between 0.01 and 1000000")] decimal PricePerNight,
    [Range(1, 100, ErrorMessage = "Capacity must be between 1 and 100")] int Capacity,
    [MaxLength(1000, ErrorMessage = "Image URL cannot exceed 1000 characters")] string? ImageUrl,
    [MaxLength(200, ErrorMessage = "Location cannot exceed 200 characters")] string? Location,
    [MaxLength(300, ErrorMessage = "Address cannot exceed 300 characters")] string? Address,
    List<string>? Amenities
);