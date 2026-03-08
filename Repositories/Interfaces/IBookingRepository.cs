using BookingApp.Api.Models;

namespace BookingApp.Api.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<bool> HasOverlapAsync(int roomId, DateOnly checkIn, DateOnly checkOut, CancellationToken ct = default);
    Task<Booking> AddAsync(Booking booking, CancellationToken ct = default);
    Task<List<Booking>> GetForUserAsync(int userId, CancellationToken ct = default);
}