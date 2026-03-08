using BookingApp.Api.DTOs.Bookings;

namespace BookingApp.Api.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateAsync(int userId, CreateBookingRequest req, CancellationToken ct = default);
    Task<List<BookingResponse>> MyBookingsAsync(int userId, CancellationToken ct = default);
    Task CancleAsync(int bookingId, int userId, bool isAdmin, CancellationToken ct = default);    
    Task ConfirmAsync(int bookingId, CancellationToken ct = default);
    Task<List<AdminBookingResponse>> GetAllAsync(string? status, CancellationToken ct = default);
}