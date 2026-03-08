using BookingApp.Api.DTOs.Rooms;
using BookingApp.Api.Models;
using BookingApp.Api.Repositories.Interfaces;
using BookingApp.Api.Services.Interfaces;

namespace BookingApp.Api.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _rooms;
    public RoomService(IRoomRepository rooms) => _rooms = rooms;

    public async Task<List<RoomResponse>> GetAllAsync(bool includeInactive, CancellationToken ct = default)
    {
        var list = await _rooms.GetAllAsync(includeInactive, ct);
        return list.Select(ToResponse).ToList();
    }

    public async Task<RoomResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var room = await _rooms.GetByIdAsync(id, ct);
        return room is null ? null : ToResponse(room);
    }

    public async Task<RoomResponse> CreateAsync(CreateRoomRequest req, CancellationToken ct = default)
    {
        Validate(req.Name, req.PricePerNight, req.Capacity);

        var room = new Room
        {
            Name = req.Name.Trim(),
            Description = req.Description?.Trim(),
            PricePerNight = req.PricePerNight,
            Capacity = req.Capacity,
            IsActive = true
        };

        await _rooms.AddAsync(room, ct);
        return ToResponse(room);
    }

    public async Task<RoomResponse?> UpdateAsync(int id, UpdateRoomRequest req, CancellationToken ct = default)
    {
        Validate(req.Name, req.PricePerNight, req.Capacity);

        var room = await _rooms.GetByIdAsync(id, ct);
        if (room is null) return null;

        room.Name = req.Name.Trim();
        room.Description = req.Description?.Trim();
        room.PricePerNight = req.PricePerNight;
        room.Capacity = req.Capacity;
        room.IsActive = req.IsActive;

        await _rooms.UpdateAsync(room, ct);
        return ToResponse(room);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var room = await _rooms.GetByIdAsync(id, ct);
        if (room is null) return false;

        await _rooms.DeleteAsync(room, ct);
        return true;
    }

    private static void Validate(string name, decimal price, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");
        if (price < 0)
            throw new ArgumentException("PricePerNight cannot be negative.");
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be > 0.");
    }

    private static RoomResponse ToResponse(Room r) =>
        new(r.Id, r.Name, r.Description, r.PricePerNight, r.Capacity, r.IsActive);
}