
using GrowAndGlow.Api.DTOs.User;
using GrowAndGlow.Api.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrowAndGlow.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // --------------------------------------------------------
        // GET: api/users/me
        // Returns logged-in user's profile
        // --------------------------------------------------------
        [HttpGet("me")]
        public async Task<ActionResult<UserResponseDto>> GetProfile()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Invalid user token.");

            var userId = int.Parse(userIdClaim);
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return NotFound("User not found.");

            return Ok(new UserResponseDto
            {
                UserId = user.UserId,
                DisplayName = user.DisplayName,
                Email = user.Email,
                ZodiacSign = user.ZodiacSign,
                CreatedAt = user.CreatedAt
            });
        }
    }
}