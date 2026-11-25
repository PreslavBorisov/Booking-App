using BookingApp.Api.Auth;
using BookingApp.Api.Auth.Interfaces;
using BookingApp.Api.Data;
using BookingApp.Api.Repositories;
using BookingApp.Api.Repositories.Interfaces;
using BookingApp.Api.Services;
using BookingApp.Api.Services.Interfaces;
using BookingApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.AddBookingApi("BookingApp Identity API");
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var app = builder.Build();
await DbSeeder.SeedAdminAsync(app.Services);
app.MapBookingApi();
app.Run();
