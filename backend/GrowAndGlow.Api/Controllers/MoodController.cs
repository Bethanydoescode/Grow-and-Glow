using GrowAndGlow.Api.DTOs.Mood;
using GrowAndGlow.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrowAndGlow.Api.Controllers
{
    [Route("api/mood")]
    [ApiController]
    [Authorize]
    public class MoodController : ControllerBase
    {
        private readonly IMoodService _moodService;

        public MoodController(IMoodService moodService)
        {
            _moodService = moodService;
        }

        // Helper: Retrieve logged-in user's ID from token
        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("Invalid token — userId claim missing.");

            return int.Parse(userIdClaim);
        }

        // ------- CREATE --------------------
        // POST: api/mood
        [HttpPost]
        public async Task<ActionResult<MoodEntryResponseDto>> CreateMood([FromBody] MoodEntryRequestDto dto)
        {
            var userId = GetUserId();
            var response = await _moodService.CreateMoodEntryAsync(userId, dto);
            return Ok(response);
        }

        // ------- READ (ALL HISTORY) -------
        // GET: api/mood
        [HttpGet]
        public async Task<ActionResult<List<MoodEntryResponseDto>>> GetMoodHistory()
        {
            var userId = GetUserId();
            var response = await _moodService.GetMoodHistoryAsync(userId);
            return Ok(response);
        }

        // ------- READ (LAST 7 DAYS) -------
        // GET: api/mood/last7
        [HttpGet("last7")]
        public async Task<ActionResult<List<MoodEntryResponseDto>>> GetLast7Days()
        {
            var userId = GetUserId();
            var response = await _moodService.GetLast7DaysAsync(userId);
            return Ok(response);
        }

        // ------- READ (STREAK) ------------
        // GET: api/mood/streak
        [HttpGet("streak")]
        public async Task<ActionResult<int>> GetStreak()
        {
            var userId = GetUserId();
            var streak = await _moodService.GetStreakCountAsync(userId);
            return Ok(streak);
        }

        // ------- UPDATE -------------------
        // PUT: api/mood/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<MoodEntryResponseDto>> UpdateMood(int id, [FromBody] MoodEntryRequestDto dto)
        {
            var userId = GetUserId();
            var response = await _moodService.UpdateMoodEntryAsync(userId, id, dto);

            return response == null
                ? NotFound("Entry not found or unauthorized.")
                : Ok(response);
        }

        // ------- DELETE -------------------
        // DELETE: api/mood/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMood(int id)
        {
            var userId = GetUserId();
            var deleted = await _moodService.DeleteMoodEntryAsync(userId, id);

            return deleted ? NoContent() : NotFound("Entry not found or unauthorized.");
        }
    }
}
