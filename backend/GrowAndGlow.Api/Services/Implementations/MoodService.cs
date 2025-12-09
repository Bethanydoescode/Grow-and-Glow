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


            return new MoodEntryResponseDto
            {
                EntryId = savedEntry.EntryId,
                MoodEmoji = savedEntry.MoodEmoji,
                Note = savedEntry.Note,
                Tags = savedEntry.Tags,
                EntryDate = savedEntry.EntryDate,
                UserId = savedEntry.UserId
            };
        }

        public async Task<List<MoodEntryResponseDto>> GetMoodHistoryAsync(int userId)
        {
            var entries = await _moodRepository.GetMoodEntriesForUserAsync(userId);

            return [.. entries.Select(m => new MoodEntryResponseDto
            {
                EntryId = m.EntryId,
                MoodEmoji = m.MoodEmoji,
                Note = m.Note,
                Tags = m.Tags,
                EntryDate = m.EntryDate,
                UserId = m.UserId
            })];
        }

        public async Task<int> GetStreakCountAsync(int userId)
        {
            return await _moodRepository.GetStreakCountAsync(userId);
        }

        public async Task<List<MoodEntryResponseDto>> GetLast7DaysAsync(int userId)
        {
            var entries = await _moodRepository.GetMoodEntriesLast7DaysAsync(userId);

            return [.. entries.Select(m => new MoodEntryResponseDto
            {
                EntryId = m.EntryId,
                MoodEmoji = m.MoodEmoji,
                Note = m.Note,
                Tags = m.Tags,
                EntryDate = m.EntryDate,
                UserId = m.UserId
            })];
        }
    }
}