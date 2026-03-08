using BookingApp.Api.DTOs.Rooms;
using BookingApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _rooms;

    public RoomController(IRoomService rooms)
    {
        _rooms = rooms;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoomResponse>>> GetAll([FromQuery] bool includeInactive, CancellationToken ct)
    {
        var res = await _rooms.GetAllAsync(includeInactive, ct);
        return Ok(res);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoomResponse>> GetById(int id, CancellationToken ct)
    {
        var room = await _rooms.GetByIdAsync(id, ct);
        return room is null ? NotFound() : Ok(room);
    }

    [HttpGet("{id:int}/availability")]
    public async Task<ActionResult<RoomAvailabilityResponse>> CheckAvailability(
        int id,
        [FromQuery] DateOnly checkIn,
        [FromQuery] DateOnly checkOut,
        CancellationToken ct)
    {
        var result = await _rooms.CheckAvailabilityAsync(id, checkIn, checkOut, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<RoomResponse>> Create(CreateRoomRequest req, CancellationToken ct)
    {
        var created = await _rooms.CreateAsync(req, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<RoomResponse>> Update(int id, UpdateRoomRequest req, CancellationToken ct)
    {
        var updated = await _rooms.UpdateAsync(id, req, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _rooms.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}