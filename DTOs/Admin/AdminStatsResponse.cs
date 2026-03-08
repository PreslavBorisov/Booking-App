namespace BookingApp.Api.DTOs.Admin;

public record AdminStatsResponse(
    int TotalRooms,
    int ActiveRooms,
    int TotalUsers,
    int TotalBookings,
    int PendingBookings,
    int ConfirmedBookings,
    int CancelledBookings
);