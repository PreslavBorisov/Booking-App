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
            b.Status != BookingStatus.Cancelled &&
            b.CheckIn < checkOut &&
            checkIn < b.CheckOut, ct);

    public async Task<Booking> AddAsync(Booking booking, CancellationToken ct = default)
    {
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync(ct);
        return booking;
    }

    public Task<List<Booking>> GetForUserAsync(int userId, CancellationToken ct = default)
        => _db.Bookings
            .AsNoTracking()
            .Include(b => b.Room)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.Id)
            .ToListAsync(ct);
    public async Task<bool> IsRoomAvailableAsync(int roomId, DateOnly checkIn, DateOnly checkOut, CancellationToken ct = default)
    {
        var hasOverlap = await HasOverlapAsync(roomId, checkIn, checkOut, ct);
        return !hasOverlap;
    }

    public Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return _db.Bookings.FirstOrDefaultAsync(b => b.Id == id, ct);
    }

    public async Task DeleteAsync(Booking booking, CancellationToken ct = default)
    {
        _db.Bookings.Remove(booking);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Booking booking, CancellationToken ct = default)
    {
        _db.Bookings.Update(booking);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<List<Booking>> GetAllAsync(string? status, CancellationToken ct = default)
    {
        IQueryable<Booking> query = _db.Bookings
        .AsNoTracking()
        .Include(b => b.User)
        .Include(b => b.Room)
        .OrderByDescending(b => b.Id)
        .AsQueryable();

        if(!string.IsNullOrWhiteSpace(status) && Enum.TryParse<BookingStatus>(status, true, out var parsedStatus))
        {
            query = query.Where(b => b.Status == parsedStatus);
        }

        return await query.ToListAsync(ct);
    }
}