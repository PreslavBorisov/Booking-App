using BookingApp.Api.Auth.Interfaces;
using BookingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider services)
    {
        using IServiceScope scope = services.CreateScope();

        AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        IPasswordHasher hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        await db.Database.MigrateAsync();

        string? email = configuration["SeedAdmin:Email"];
        string? password = configuration["SeedAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return;

        email = email.Trim().ToLowerInvariant();

        AppUser? existingAdmin = await db.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (existingAdmin is null)
        {
            AppUser admin = new AppUser
            {
                Email = email,
                PasswordHash = hasher.Hash(password),
                Role = "Admin"
            };

            db.Users.Add(admin);
            await db.SaveChangesAsync();
            return;
        }

        if (existingAdmin.Role != "Admin")
        {
            existingAdmin.Role = "Admin";
            await db.SaveChangesAsync();
        }
    }
}