using BookingApp.Api.DTOs.Bookings;
using BookingApp.Api.Models;
using BookingApp.Api.Repositories.Interfaces;
using BookingApp.Api.Services.Interfaces;

namespace BookingApp.Api.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookings;
    private readonly IRoomRepository _rooms;

    public BookingService(IBookingRepository bookings, IRoomRepository rooms)
    {
        _bookings = bookings;
        _rooms = rooms;
    }

    public async Task<BookingResponse> CreateAsync(int userId, CreateBookingRequest req, CancellationToken ct = default)
    {
        if (req.CheckOut <= req.CheckIn)
            throw new ArgumentException("CheckOut must be after CheckIn.");

        var room = await _rooms.GetByIdAsync(req.RoomId, ct);
        if (room is null || !room.IsActive)
            throw new ArgumentException("Room not found or inactive.");

        if (await _bookings.HasOverlapAsync(req.RoomId, req.CheckIn, req.CheckOut, ct))
            throw new InvalidOperationException("Room is already booked for these dates.");

        var booking = new Booking
        {
            RoomId = req.RoomId,
            UserId = userId,
            CheckIn = req.CheckIn,
            CheckOut = req.CheckOut
        };

        await _bookings.AddAsync(booking, ct);

        return new BookingResponse(booking.Id, booking.RoomId, booking.UserId, booking.CheckIn, booking.CheckOut);
    }

    public async Task<List<BookingResponse>> MyBookingsAsync(int userId, CancellationToken ct = default)
    {
        var list = await _bookings.GetForUserAsync(userId, ct);
        return list.Select(b => new BookingResponse(b.Id, b.RoomId, b.UserId, b.CheckIn, b.CheckOut)).ToList();
    }
}