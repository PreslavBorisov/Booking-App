using System.ComponentModel.DataAnnotations;

namespace BookingApp.Api.DTOs.Auth;

public record RegisterRequest(
    [property: Required, EmailAddress] string Email, [property: Required, MinLength(6), MaxLength(20)] string Password);