using BookingApp.Api.Data;
using BookingApp.Api.Models;
using BookingApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Api.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _db;
    public BookingRepository(AppDbContext db) => _db = db;

    public Task<bool> HasOverlapAsync(int roomId, DateOnly checkIn, DateOnly checkOut, CancellationToken ct = default)
        => _db.Bookings.AsNoTracking().AnyAsync(b =>
            b.RoomId == roomId &&
            b.CheckIn < checkOut &&
            checkIn < b.CheckOut, ct);

    public async Task<Booking> AddAsync(Booking booking, CancellationToken ct = default)
    {
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync(ct);
        return booking;
    }

    public Task<List<Booking>> GetForUserAsync(int userId, CancellationToken ct = default)
        => _db.Bookings.AsNoTracking()
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.Id)
            .ToListAsync(ct);
    public async Task<bool> IsRoomAvailableAsync(int roomId, DateOnly checkIn, DateOnly checkOut, CancellationToken ct = default)
    {
        var hasOverlap = await HasOverlapAsync(roomId, checkIn, checkOut, ct);
        return !hasOverlap;
    }
}