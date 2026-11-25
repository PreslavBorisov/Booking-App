using BookingApp.Api.DTOs.Admin;

namespace BookingApp.Api.Services.Interfaces;

public interface IAdminService
{
    Task<AdminStatsResponse> GetStatsAsync(CancellationToken ct = default);
}