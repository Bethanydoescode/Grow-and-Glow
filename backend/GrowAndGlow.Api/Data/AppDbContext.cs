using System.Text.Json;
using GrowAndGlow.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace GrowAndGlow.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // --------------------------------------------------------
        // DbSets - Database Tables
        // --------------------------------------------------------
        public DbSet<User> Users { get; set; }
        public DbSet<MoodEntry> MoodEntries { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

// --------------------------------------------------------
// Model Configuration
// --------------------------------------------------------
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // --------------------------------------------
    // Value Converter - Store List<string> as JSON
    // --------------------------------------------
    var tagsConverter = new ValueConverter<List<string>?, string>(
        v => JsonSerializer.Serialize(v ?? new List<string>()),
        v => string.IsNullOrEmpty(v)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(v) ?? new List<string>()
    );

    // --------------------------------------------
    // Value Comparer - Ensures EF change tracking works correctly
    // --------------------------------------------
    var tagsComparer = new ValueComparer<List<string>>(
        (c1, c2) => c1!.SequenceEqual(c2!), // compare two lists
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())), // hash
        c => c.ToList() // clone
    );

    // Apply converter + value comparer
    modelBuilder.Entity<MoodEntry>()
        .Property(m => m.Tags)
        .HasConversion(tagsConverter)
        .Metadata.SetValueComparer(tagsComparer);

    // --------------------------------------------
    // Indexes & Constraints
    // --------------------------------------------
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

    // --------------------------------------------
    // Relationships
    // --------------------------------------------
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
