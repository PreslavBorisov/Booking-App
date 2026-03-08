using BookingApp.Api.Data;
using BookingApp.Api.Models;
using BookingApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Api.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _db;
    public RoomRepository(AppDbContext db) => _db = db;

    public async Task<List<Room>> GetAllAsync(bool includeInactive, CancellationToken ct = default)
    {
        var q = _db.Rooms.AsNoTracking();
        if (!includeInactive)
            q = q.Where(r => r.IsActive);
        return await q.OrderBy(r => r.Id).ToListAsync(ct);
    }

    public Task<Room?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Rooms.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<Room> AddAsync(Room room, CancellationToken ct = default)
    {
        _db.Rooms.Add(room);
        await _db.SaveChangesAsync(ct);
        return room;
    }

    public async Task UpdateAsync(Room room, CancellationToken ct = default)
    {
        _db.Rooms.Update(room);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Room room, CancellationToken ct = default)
    {
        _db.Rooms.Remove(room);
        await _db.SaveChangesAsync(ct);
    }
}