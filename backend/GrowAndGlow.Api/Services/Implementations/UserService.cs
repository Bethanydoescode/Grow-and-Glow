using GrowAndGlow.Api.DTOs.User;
using GrowAndGlow.Api.Repository.Interfaces;
using GrowAndGlow.Api.Services.Interfaces;

namespace GrowAndGlow.Api.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new Exception("User not found.");

            return new UserResponseDto
            {
                UserId = user.UserId,
                DisplayName = user.DisplayName,
                Email = user.Email,
                ZodiacSign = user.ZodiacSign,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
