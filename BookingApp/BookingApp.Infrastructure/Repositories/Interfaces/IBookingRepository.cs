using BookingApp.Api.Models;

namespace BookingApp.Api.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<bool> HasOverlapAsync(int roomId, DateOnly checkIn, DateOnly checkOut, CancellationToken ct = default);
    Task<Booking> AddAsync(Booking booking, CancellationToken ct = default);
    Task<List<Booking>> GetForUserAsync(int userId, CancellationToken ct = default);
    Task<bool> IsRoomAvailableAsync(int roomId, DateOnly checkIn, DateOnly checkOut, CancellationToken ct = default);
    Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default);
    Task DeleteAsync(Booking booking, CancellationToken ct = default);
    Task UpdateAsync(Booking booking, CancellationToken ct = default);
    Task<List<Booking>> GetAllAsync(string? status, CancellationToken ct = default);
}