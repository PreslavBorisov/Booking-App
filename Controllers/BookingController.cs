using System.Security.Claims;
using BookingApp.Api.DTOs.Bookings;
using BookingApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookings;
    public BookingsController(IBookingService bookings) => _bookings = bookings;

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
        try
        {
            var res = await _bookings.CreateAsync(GetUserId(), req, ct);
            return Ok(res);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    private int GetUserId()
    {
        var sub = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(sub, out var id) ? id : throw new InvalidOperationException("Invalid token.");
    }
}