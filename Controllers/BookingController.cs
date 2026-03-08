using System.Security.Claims;
using BookingApp.Api.DTOs.Bookings;
using BookingApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookings;

    public BookingController(IBookingService bookings)
    {
        _bookings = bookings;
    }

    [HttpGet("me")]
    public async Task<ActionResult<List<BookingResponse>>> MyBookings(CancellationToken ct)
    {
        var userId = GetUserId();
        var res = await _bookings.MyBookingsAsync(userId, ct);
        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<BookingResponse>> Create(CreateBookingRequest req, CancellationToken ct)
    {
        var res = await _bookings.CreateAsync(GetUserId(), req, ct);
        return Ok(res);
    }

    private int GetUserId()
    {
        var sub = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(sub, out var id))
            throw new UnauthorizedAccessException("Invalid token.");

        return id;
    }
}