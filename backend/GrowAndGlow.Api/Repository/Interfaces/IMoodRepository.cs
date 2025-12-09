using GrowAndGlow.Api.Models;

namespace GrowAndGlow.Api.Repository.Interfaces
{
    public interface IMoodRepository
    {
        // CREATE
        Task<MoodEntry> CreateMoodEntryAsync(MoodEntry entry);

        // READ
        Task<MoodEntry?> GetMoodEntryByIdAsync(int entryId);
        Task<List<MoodEntry>> GetMoodEntriesForUserAsync(int userId);
        Task<List<MoodEntry>> GetMoodEntriesLast7DaysAsync(int userId);
        Task<int> GetStreakCountAsync(int userId);

        // UPDATE
        Task UpdateMoodEntryAsync(MoodEntry entry);

        // DELETE
        Task DeleteMoodEntryAsync(MoodEntry entry);

        // SAVE CHANGES — Called only by service layer
        Task SaveChangesAsync();
    }
}
