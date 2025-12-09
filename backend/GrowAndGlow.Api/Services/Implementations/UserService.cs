using GrowAndGlow.Api.DTOs.User;
using GrowAndGlow.Api.Models;
using GrowAndGlow.Api.Repository.Interfaces;
using GrowAndGlow.Api.Services.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace GrowAndGlow.Api.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new Exception("User not found.");

            return MapToResponse(user);
        }

        public async Task<UserResponseDto> UpdateUserProfileAsync(int userId, UpdateProfileRequestDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new Exception("User not found.");

            ValidateEmailChange(dto, user);
            ValidatePasswordChange(dto, user);

            ApplyProfileUpdates(dto, user);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return MapToResponse(user);
        }

        // ---------------- Helper Methods ----------------

        private static UserResponseDto MapToResponse(User user)
        {
            return new UserResponseDto
            {
                UserId = user.UserId,
                DisplayName = user.DisplayName,
                Email = user.Email,
                ZodiacSign = user.ZodiacSign,
                CreatedAt = user.CreatedAt
            };
        }

        private void ValidateEmailChange(UpdateProfileRequestDto dto, User user)
        {
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
            {
                var exists = _userRepository.UserExistsByEmailAsync(dto.Email).Result;
                if (exists)
                    throw new Exception("Email is already used by another user.");
            }
        }

        private static void ValidatePasswordChange(UpdateProfileRequestDto dto, User user)
        {
            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                if (string.IsNullOrEmpty(dto.CurrentPassword) || !BC.Verify(dto.CurrentPassword, user.PasswordHash))
                    throw new Exception("Current password is incorrect.");
            }
        }

        private static void ApplyProfileUpdates(UpdateProfileRequestDto dto, User user)
        {
            if (!string.IsNullOrEmpty(dto.DisplayName))
                user.DisplayName = dto.DisplayName;

            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;

            if (dto.ZodiacSign.HasValue)
                user.ZodiacSign = dto.ZodiacSign.Value;

            if (!string.IsNullOrEmpty(dto.NewPassword))
                user.PasswordHash = BC.HashPassword(dto.NewPassword);
        }
        public async Task<bool> DeleteUserAsync(int userId)
{
    var user = await _userRepository.GetByIdAsync(userId);

    if (user == null)
        return false;

    await _userRepository.DeleteUserAsync(user);
    await _userRepository.SaveChangesAsync();

    return true;
}

    }
}
