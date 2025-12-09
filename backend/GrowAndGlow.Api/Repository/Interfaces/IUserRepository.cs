using GrowAndGlow.Api.Models;

namespace GrowAndGlow.Api.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<bool> UserExistsByEmailAsync(string email);

        Task UpdateAsync(User user);     
                Task DeleteUserAsync(User user);
 
        Task SaveChangesAsync();           
    }
}
