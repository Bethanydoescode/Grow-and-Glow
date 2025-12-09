using GrowAndGlow.Api.DTOs.User;
using GrowAndGlow.Api.Services.Interfaces;
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
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // ---------------- HELPER TO GET USER ID ----------------
        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("Invalid token — no userId claim found.");

            return int.Parse(userIdClaim);
        }

        // ---------------- GET PROFILE ----------------
        // GET: api/users/me
        [HttpGet("me")]
        public async Task<ActionResult<UserResponseDto>> GetProfile()
        {
            var userId = GetUserId();
            var response = await _userService.GetUserByIdAsync(userId);
            return Ok(response);
        }

        // ---------------- UPDATE PROFILE ----------------
        // PUT: api/users/me
        [HttpPut("me")]
        public async Task<ActionResult<UserResponseDto>> UpdateProfile([FromBody] UpdateProfileRequestDto dto)
        {
            var userId = GetUserId();

            try
            {
                var response = await _userService.UpdateUserProfileAsync(userId, dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        // DELETE: api/users/me
[HttpDelete("me")]
public async Task<IActionResult> DeleteAccount()
{
    var userId = GetUserId();
    var result = await _userService.DeleteUserAsync(userId);

    return result ? NoContent() : NotFound("User not found.");
}

    }
}
