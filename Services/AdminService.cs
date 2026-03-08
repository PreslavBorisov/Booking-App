using BookingApp.Api.Data;
using BookingApp.Api.DTOs.Admin;
using BookingApp.Api.Models;
using BookingApp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Api.Services;

public class AdminService : IAdminService
{
    private readonly AppDbContext _db;

    public AdminService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AdminStatsResponse> GetStatsAsync(CancellationToken ct = default)
    {
        var totalRooms = await _db.Rooms.CountAsync(ct);
        var activeRooms = await _db.Rooms.CountAsync(r => r.IsActive, ct);
        var totalUsers = await _db.Users.CountAsync(ct);
        var totalBookings = await _db.Bookings.CountAsync(ct);
        var pendingBookings = await _db.Bookings.CountAsync(b => b.Status == BookingStatus.Pending, ct);
        var confirmedBookings = await _db.Bookings.CountAsync(b => b.Status == BookingStatus.Confirmed, ct);
        var cancelledBookings = await _db.Bookings.CountAsync(b => b.Status == BookingStatus.Cancelled, ct);

        return new AdminStatsResponse(
            totalRooms,
            activeRooms,
            totalUsers,
            totalBookings,
            pendingBookings,
            confirmedBookings,
            cancelledBookings
        );
    }
}