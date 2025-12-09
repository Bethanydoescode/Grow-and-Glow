using GrowAndGlow.Api.Models;

namespace GrowAndGlow.Api.Repository.Interfaces
{
    public interface IMoodRepository
    {
        Task<MoodEntry> CreateMoodEntryAsync(MoodEntry entry);
        Task<List<MoodEntry>> GetMoodEntriesForUserAsync(int userId);
        Task<List<MoodEntry>> GetMoodEntriesLast7DaysAsync(int userId);
        Task<int> GetStreakCountAsync(int userId);
    }
}
