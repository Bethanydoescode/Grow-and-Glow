using GrowAndGlow.Api.DTOs.Mood;

namespace GrowAndGlow.Api.Services.Interfaces
{
    public interface IMoodService
    {
        Task<MoodEntryResponseDto> CreateMoodEntryAsync(int userId, MoodEntryRequestDto request);
        Task<List<MoodEntryResponseDto>> GetMoodHistoryAsync(int userId);
        Task<int> GetStreakCountAsync(int userId);
        Task<List<MoodEntryResponseDto>> GetLast7DaysAsync(int userId);
    }
}