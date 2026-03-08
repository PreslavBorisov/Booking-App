using BookingApp.Api.DTOs.Rooms;

namespace BookingApp.Api.Services.Interfaces;

public interface IRoomService
{
    Task<List<RoomResponse>> GetAllAsync(bool includeInactive, CancellationToken ct = default);
    Task<RoomResponse?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<RoomResponse> CreateAsync(CreateRoomRequest req, CancellationToken ct = default);
    Task<RoomResponse?> UpdateAsync(int id, UpdateRoomRequest req, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}