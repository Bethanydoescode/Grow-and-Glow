using GrowAndGlow.Api.DTOs.Auth;
using GrowAndGlow.Api.Models;
using GrowAndGlow.Api.Repository.Interfaces;
using GrowAndGlow.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GrowAndGlow.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            if (await _userRepository.UserExistsByEmailAsync(request.Email))
                throw new Exception("Email already registered.");

            var user = new User
            {
                DisplayName = request.DisplayName,
                Email = request.Email,
                ZodiacSign = request.ZodiacSign,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            await _userRepository.CreateUserAsync(user);

            var accessToken = _tokenService.CreateAccessToken(user);
            var refreshToken = _tokenService.CreateRefreshToken(user.UserId);
            await _refreshTokenRepository.CreateTokenAsync(refreshToken);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                DisplayName = user.DisplayName,
                Email = user.Email,
                ZodiacSign = user.ZodiacSign,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email)
                ?? throw new Exception("Invalid email or password.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid email or password.");

            var accessToken = _tokenService.CreateAccessToken(user);
            var refreshToken = _tokenService.CreateRefreshToken(user.UserId);
            await _refreshTokenRepository.CreateTokenAsync(refreshToken);

            return new AuthResponseDto
            {
                UserId = user.UserId,
                DisplayName = user.DisplayName,
                Email = user.Email,
                ZodiacSign = user.ZodiacSign,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
