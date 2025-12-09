// --- Models/User.cs ---
using System.ComponentModel.DataAnnotations;

namespace GrowAndGlow.Api.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public required string DisplayName { get; set; }

        public string? ZodiacSign { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public List<MoodEntry> MoodEntries { get; set; } = [];
        public List<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
