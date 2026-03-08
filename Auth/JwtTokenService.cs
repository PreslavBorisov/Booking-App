using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookingApp.Api.Auth.Interfaces;
using BookingApp.Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookingApp.Api.Auth;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _opts;

    public JwtTokenService(IOptions<JwtOptions> opts) => _opts = opts.Value;

    public string CreateAccessToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _opts.Issuer,
            audience: _opts.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_opts.ExpiresMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}