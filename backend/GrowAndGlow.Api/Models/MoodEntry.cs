// --- Models/MoodEntry.cs ---
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrowAndGlow.Api.Models
{
    public class MoodEntry
    {
        [Key]
        public int EntryId { get; set; }

        [Required]
        public required string MoodEmoji { get; set; }

        public string? Note { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public DateTime EntryDate { get; set; } = DateTime.UtcNow.Date;

        // Foreign Key
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
