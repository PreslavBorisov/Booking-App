using BookingApp.Api.Data;
using BookingApp.Api.DTOs.Common;
using BookingApp.Api.DTOs.Rooms;
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
        IQueryable<Room> q = _db.Rooms.AsNoTracking();
        if (!includeInactive)
            q = q.Where(r => r.IsActive);
        return await q.OrderBy(r => r.Id).ToListAsync(ct);
    }

    public async Task<PagedResult<Room>> QueryAsync(RoomQuery query, CancellationToken ct = default)
    {
        IQueryable<Room> rooms = _db.Rooms.AsNoTracking().AsQueryable();

        rooms = rooms.Where(r => r.IsActive);

        if (query.Capacity.HasValue)
        {
            rooms = rooms.Where(r => r.Capacity >= query.Capacity.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            rooms = rooms.Where(r => r.PricePerNight <= query.MaxPrice.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            string? search = query.Search.Trim().ToLower();

            rooms = rooms.Where(r =>
                r.Name.ToLower().Contains(search) ||
                (r.Description != null && r.Description.ToLower().Contains(search)) ||
                (r.Location != null && r.Location.ToLower().Contains(search)) ||
                (r.Address != null && r.Address.ToLower().Contains(search)));
        }

        if (!string.IsNullOrWhiteSpace(query.Amenity))
        {
            var amenity = query.Amenity.Trim().ToLower();
            rooms = rooms.Where(r => r.Amenities.Any(a => a.ToLower() == amenity));
        }

        if (query.CheckIn.HasValue && query.CheckOut.HasValue)
        {
            DateOnly checkIn = query.CheckIn.Value;
            DateOnly checkOut = query.CheckOut.Value;

            rooms = rooms.Where(r =>
                !_db.Bookings.Any(b =>
                    b.RoomId == r.Id &&
                    b.Status != BookingStatus.Cancelled &&
                    b.CheckIn < checkOut &&
                    checkIn < b.CheckOut
                ));
        }

        string? sortBy = query.SortBy?.Trim().ToLower();
        string? sortOrder = query.SortOrder?.Trim().ToLower() == "desc" ? "desc" : "asc";

        rooms = sortBy switch
        {
            "price" => sortOrder == "desc"
                ? rooms.OrderByDescending(r => r.PricePerNight)
                : rooms.OrderBy(r => r.PricePerNight),

            "capacity" => sortOrder == "desc"
                ? rooms.OrderByDescending(r => r.Capacity)
                : rooms.OrderBy(r => r.Capacity),

            "newest" => sortOrder == "desc"
                ? rooms.OrderBy(r => r.CreatedAtUtc)
                : rooms.OrderByDescending(r => r.CreatedAtUtc),

            _ => rooms.OrderBy(r => r.Id)
        };

        var totalCount = await rooms.CountAsync(ct);

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize < 1 ? 10 : query.PageSize;

        var items = await rooms
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Room>
        {
            Items = items,
            TotalCount = totalCount
        };
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