namespace BookingApp.Api.DTOs.Auth;

public record AuthResponse(string AccessToken, string Email, string Role);