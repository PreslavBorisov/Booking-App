using BookingApp.Api.DTOs.Rooms;
using BookingApp.Api.Models;
using BookingApp.Api.DTOs.Common;

namespace BookingApp.Api.Repositories.Interfaces;

public interface IRoomRepository
{
    Task<List<Room>> GetAllAsync(bool includeInactive, CancellationToken ct = default);
    Task<PagedResult<Room>> QueryAsync(RoomQuery query, CancellationToken ct = default);
    Task<Room?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Room> AddAsync(Room room, CancellationToken ct = default);
    Task UpdateAsync(Room room, CancellationToken ct = default);
    Task DeleteAsync(Room room, CancellationToken ct = default);
    
}