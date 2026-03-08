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
        int userId = GetUserId();
        List<BookingResponse> res = await _bookings.MyBookingsAsync(userId, ct);
        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<BookingResponse>> Create(CreateBookingRequest req, CancellationToken ct)
    {
        BookingResponse res = await _bookings.CreateAsync(GetUserId(), req, ct);
        return Ok(res);
    }

    private int GetUserId()
    {
        string? sub = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(sub, out var id))
            throw new UnauthorizedAccessException("Invalid token.");

        return id;
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        await _bookings.CancleAsync(id, GetUserId(), isAdmin(), ct);
        return NoContent();
    }
    private bool isAdmin()
    {
        return User.IsInRole("Admin");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:int}/confirm")]
    public async Task<IActionResult> Confirm(int id, CancellationToken ct)
    {
        await _bookings.ConfirmAsync(id, ct);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<AdminBookingResponse>>> GetAll(
        [FromQuery] string? status,
        CancellationToken ct)
    {
        var res = await _bookings.GetAllAsync(status, ct);
        return Ok(res);   
    }
    
}