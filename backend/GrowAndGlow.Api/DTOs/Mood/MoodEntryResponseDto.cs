namespace GrowAndGlow.Api.DTOs.Mood
{
    public class MoodEntryResponseDto
    {
        public int EntryId { get; set; }
        public string MoodEmoji { get; set; } = string.Empty;
        public string? Note { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public DateTime EntryDate { get; set; }
        public int UserId { get; set; }
    }
}


//➡ This is what your API will return when a mood entry is saved OR retrieved.