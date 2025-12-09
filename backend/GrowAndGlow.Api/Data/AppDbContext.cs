// --- Data/AppDbContext.cs ---
using Microsoft.EntityFrameworkCore;
using GrowAndGlow.Api.Models;

namespace GrowAndGlow.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<MoodEntry> MoodEntries { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationships
            // Unique email constraint
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

    // Relationships and cascade behavior
    modelBuilder.Entity<MoodEntry>()
        .HasOne(m => m.User)
        .WithMany(u => u.MoodEntries)
        .HasForeignKey(m => m.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<RefreshToken>()
        .HasOne(r => r.User)
        .WithMany(u => u.RefreshTokens)
        .HasForeignKey(r => r.UserId)
        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
