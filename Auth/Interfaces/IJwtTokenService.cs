using BookingApp.Api.Models;

namespace BookingApp.Api.Auth.Interfaces;

public interface IJwtTokenService
{
    string CreateAccessToken(AppUser user);
}