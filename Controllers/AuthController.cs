using BookingApp.Api.DTOs.Auth;
using BookingApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req, CancellationToken ct)
    {
        AuthResponse res = await _auth.RegisterAsync(req, ct);
        return Ok(res);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest req, CancellationToken ct)
    {
        AuthResponse res = await _auth.LoginAsync(req, ct);
        return Ok(res);        
    }
}