using BookingApp.Api.Data;
using BookingApp.Api.Models;
using BookingApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db) => _db = db;

    public Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<AppUser> AddAsync(AppUser user, CancellationToken cancellationToken = default)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);
        return user;
    }
}