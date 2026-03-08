using BookingApp.Api.DTOs.Rooms;
using BookingApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _rooms;
    public RoomsController(IRoomService rooms) => _rooms = rooms;

    // Public: всички виждат активните стаи
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

    // Admin only: create/update/delete
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<RoomResponse>> Create(CreateRoomRequest req, CancellationToken ct)
    {
        try
        {
            var created = await _rooms.CreateAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<RoomResponse>> Update(int id, UpdateRoomRequest req, CancellationToken ct)
    {
        try
        {
            var updated = await _rooms.UpdateAsync(id, req, ct);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _rooms.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}