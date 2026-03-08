using BookingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(e =>
        {
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Email).HasMaxLength(320).IsRequired();
            e.Property(x => x.PasswordHash).IsRequired();
            e.Property(x => x.Role).HasMaxLength(32).IsRequired();
        });

        modelBuilder.Entity<Room>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(120).IsRequired();
            e.Property(x => x.PricePerNight).HasPrecision(10, 2);
            e.Property(x => x.Capacity).IsRequired();
            e.HasIndex(x => x.Name);
        });

        modelBuilder.Entity<Booking>(e =>
        {
            e.HasOne(b => b.Room).WithMany().HasForeignKey(b => b.RoomId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(b => b.User).WithMany().HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(b => new { b.RoomId, b.CheckIn, b.CheckOut });
            e.Property(b => b.Status).HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();
        });
       
    }
}