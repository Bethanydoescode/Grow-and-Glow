
using GrowAndGlow.Api.DTOs.User;

namespace GrowAndGlow.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> GetUserByIdAsync(int userId);
    }
}
