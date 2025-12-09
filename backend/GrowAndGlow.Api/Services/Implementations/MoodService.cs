using GrowAndGlow.Api.DTOs.Mood;
using GrowAndGlow.Api.Models;
using GrowAndGlow.Api.Repository.Interfaces;
using GrowAndGlow.Api.Services.Interfaces;

namespace GrowAndGlow.Api.Services.Implementations
{
    public class MoodService : IMoodService
    {
        private readonly IMoodRepository _moodRepository;

        public MoodService(IMoodRepository moodRepository)
        {
            _moodRepository = moodRepository;
        }

        private static MoodEntryResponseDto ToDto(MoodEntry entry) =>
            new MoodEntryResponseDto
            {
                EntryId = entry.EntryId,
                MoodEmoji = entry.MoodEmoji,
                Note = entry.Note,
                Tags = entry.Tags,
                EntryDate = entry.EntryDate,
                UserId = entry.UserId
            };

        // -------- CREATE --------
        public async Task<MoodEntryResponseDto> CreateMoodEntryAsync(int userId, MoodEntryRequestDto request)
        {
            var newEntry = new MoodEntry
            {
                UserId = userId,
                MoodEmoji = request.MoodEmoji,
                Note = request.Note,
                Tags = request.Tags,
                EntryDate = DateTime.UtcNow
            };

            var savedEntry = await _moodRepository.CreateMoodEntryAsync(newEntry)
                ?? throw new Exception("Failed to save mood entry.");

            return ToDto(savedEntry);
        }

        // -------- READ --------
        public async Task<List<MoodEntryResponseDto>> GetMoodHistoryAsync(int userId)
        {
            var entries = await _moodRepository.GetMoodEntriesForUserAsync(userId);
            return entries.Select(ToDto).ToList();
        }

        public async Task<List<MoodEntryResponseDto>> GetLast7DaysAsync(int userId)
        {
            var entries = await _moodRepository.GetMoodEntriesLast7DaysAsync(userId);
            return entries.Select(ToDto).ToList();
        }

        public async Task<int> GetStreakCountAsync(int userId)
        {
            return await _moodRepository.GetStreakCountAsync(userId);
        }

        // -------- UPDATE --------
        public async Task<MoodEntryResponseDto?> UpdateMoodEntryAsync(int userId, int entryId, MoodEntryRequestDto request)
        {
            var entry = await _moodRepository.GetMoodEntryByIdAsync(entryId);

            if (entry == null || entry.UserId != userId)
                throw new UnauthorizedAccessException("Cannot update — entry does not belong to user.");

            entry.MoodEmoji = request.MoodEmoji;
            entry.Note = request.Note;
            entry.Tags = request.Tags;

            await _moodRepository.UpdateMoodEntryAsync(entry);
            await _moodRepository.SaveChangesAsync();

            return ToDto(entry);
        }

        // -------- DELETE --------
        public async Task<bool> DeleteMoodEntryAsync(int userId, int entryId)
        {
            var entry = await _moodRepository.GetMoodEntryByIdAsync(entryId);

            if (entry == null || entry.UserId != userId)
                return false;

            await _moodRepository.DeleteMoodEntryAsync(entry);
            await _moodRepository.SaveChangesAsync();

            return true;
        }
    }
}
