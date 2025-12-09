using System.ComponentModel.DataAnnotations;

namespace GrowAndGlow.Api.DTOs.Mood
{
    public class MoodEntryRequestDto
    {
        [Required]
        public string MoodEmoji { get; set; } = string.Empty;

        public string? Note { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
    }
}

//➡ This is what the frontend sends when a mood is logged.