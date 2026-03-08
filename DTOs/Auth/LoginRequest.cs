using System.ComponentModel.DataAnnotations;

namespace BookingApp.Api.DTOs.Auth;

public record LoginRequest(
    [property: Required, EmailAddress]String Email,
    [property: Required] string Password);