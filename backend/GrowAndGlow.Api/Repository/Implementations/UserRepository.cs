using GrowAndGlow.Api.Data;
using GrowAndGlow.Api.Models;
using GrowAndGlow.Api.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrowAndGlow.Api.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.MoodEntries)
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.MoodEntries)
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        // 🔹 New update method
        public Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        // 🔹 Save changes after update(s)
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        // 🔹 Delete
public async Task DeleteUserAsync(User user)
{
    _context.Users.Remove(user);
}

    }
}
