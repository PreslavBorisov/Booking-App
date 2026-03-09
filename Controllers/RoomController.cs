using BookingApp.Api.DTOs.Rooms;
using BookingApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingApp.Api.DTOs.Common;

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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoomResponse>> GetById(int id, CancellationToken ct)
    {
        RoomResponse? room = await _rooms.GetByIdAsync(id, ct);
        return room is null ? NotFound() : Ok(room);
    }

    [HttpGet("{id:int}/availability")]
    public async Task<ActionResult<RoomAvailabilityResponse>> CheckAvailability(
        int id,
        [FromQuery] DateOnly checkIn,
        [FromQuery] DateOnly checkOut,
        CancellationToken ct)
    {
        RoomAvailabilityResponse? result = await _rooms.CheckAvailabilityAsync(id, checkIn, checkOut, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<RoomResponse>> Create(CreateRoomRequest req, CancellationToken ct)
    {
        RoomResponse created = await _rooms.CreateAsync(req, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<RoomResponse>> Update(int id, UpdateRoomRequest req, CancellationToken ct)
    {
        RoomResponse? updated = await _rooms.UpdateAsync(id, req, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        bool ok = await _rooms.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
    [HttpGet]
    [HttpGet]
    public async Task<ActionResult<PagedResponse<RoomResponse>>> GetRooms(
        [FromQuery] RoomQuery query,
        CancellationToken ct)
    {
        var rooms = await _rooms.QueryAsync(query, ct);
        return Ok(rooms);
    }
    }