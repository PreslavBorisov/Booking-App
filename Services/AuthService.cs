using BookingApp.Api.Auth.Interfaces;
using BookingApp.Api.DTOs.Auth;
using BookingApp.Api.Models;
using BookingApp.Api.Repositories.Interfaces;
using BookingApp.Api.Services.Interfaces;

namespace BookingApp.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenService _jwt;

    public AuthService(IUserRepository users, IPasswordHasher hasher, IJwtTokenService jwt)
    {
        _users = users;
        _hasher = hasher;
        _jwt = jwt;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest req, CancellationToken ct = default)
    {
        var email = req.Email.Trim().ToLowerInvariant();

        var existing = await _users.GetByEmailAsync(email, ct);
        if (existing is not null)
            throw new InvalidOperationException("Email already exists.");

        var user = new AppUser
        {
            Email = email,
            PasswordHash = _hasher.Hash(req.Password),
            Role = "User"
        };

        await _users.AddAsync(user, ct);

        var token = _jwt.CreateAccessToken(user);
        return new AuthResponse(token, user.Email, user.Role);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest req, CancellationToken ct = default)
    {
        var email = req.Email.Trim().ToLowerInvariant();

        var user = await _users.GetByEmailAsync(email, ct);
        if (user is null || !_hasher.Verify(req.Password, user.PasswordHash))
            throw new InvalidOperationException("Invalid credentials.");

        var token = _jwt.CreateAccessToken(user);
        return new AuthResponse(token, user.Email, user.Role);
    }
}