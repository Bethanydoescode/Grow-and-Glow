using GrowAndGlow.Api.Data;
using GrowAndGlow.Api.Models;
using GrowAndGlow.Api.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrowAndGlow.Api.Repository.Implementations
{
    public class MoodRepository : IMoodRepository
    {
        private readonly AppDbContext _context;

        public MoodRepository(AppDbContext context)
        {
            _context = context;
        }

        // --------------------- CREATE ---------------------
        public async Task<MoodEntry> CreateMoodEntryAsync(MoodEntry entry)
        {
            _context.MoodEntries.Add(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        // ---------------------- READ ----------------------
        public async Task<MoodEntry?> GetMoodEntryByIdAsync(int entryId)
        {
            return await _context.MoodEntries.FirstOrDefaultAsync(m => m.EntryId == entryId);
        }

        public async Task<List<MoodEntry>> GetMoodEntriesForUserAsync(int userId)
        {
            return await _context.MoodEntries
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.EntryDate)
                .ToListAsync();
        }

        public async Task<List<MoodEntry>> GetMoodEntriesLast7DaysAsync(int userId)
        {
            DateTime weekAgo = DateTime.UtcNow.Date.AddDays(-7);

            return await _context.MoodEntries
                .Where(m => m.UserId == userId && m.EntryDate >= weekAgo)
                .OrderByDescending(m => m.EntryDate)
                .ToListAsync();
        }

        public async Task<int> GetStreakCountAsync(int userId)
        {
            var entries = await GetMoodEntriesForUserAsync(userId);

            int streak = 0;
            DateTime currentDay = DateTime.UtcNow.Date;

            foreach (var entry in entries)
            {
                if (entry.EntryDate.Date == currentDay)
                {
                    streak++;
                    currentDay = currentDay.AddDays(-1);
                }
                else
                {
                    break;
                }
            }

            return streak;
        }

        // --------------------- UPDATE ---------------------
        public async Task UpdateMoodEntryAsync(MoodEntry entry)
        {
            _context.MoodEntries.Update(entry);
        }

        // --------------------- DELETE ---------------------
        public async Task DeleteMoodEntryAsync(MoodEntry entry)
        {
            _context.MoodEntries.Remove(entry);
        }

        // ------------------ SAVE CHANGES ------------------
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
