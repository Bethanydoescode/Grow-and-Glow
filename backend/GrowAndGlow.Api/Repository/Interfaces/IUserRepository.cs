using GrowAndGlow.Api.Models;

namespace GrowAndGlow.Api.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<bool> UserExistsByEmailAsync(string email);
    }
}
