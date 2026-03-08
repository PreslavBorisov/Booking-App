using BookingApp.Api.Models;

namespace BookingApp.Api.Repositories.Interfaces;

public interface IRoomRepository
{
    Task<List<Room>> GetAllAsync(bool includeInactive, CancellationToken ct = default);
    Task<Room?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Room> AddAsync(Room room, CancellationToken ct = default);
    Task UpdateAsync(Room room, CancellationToken ct = default);
    Task DeleteAsync(Room room, CancellationToken ct = default);
}