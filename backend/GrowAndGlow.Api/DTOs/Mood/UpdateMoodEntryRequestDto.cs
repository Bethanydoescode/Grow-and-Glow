using System.ComponentModel.DataAnnotations;

namespace GrowAndGlow.Api.DTOs.Mood
{
    public class UpdateMoodEntryRequestDto
    {
        [MaxLength(10)]
        public string? MoodEmoji { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        public List<string>? Tags { get; set; }
    }
}
