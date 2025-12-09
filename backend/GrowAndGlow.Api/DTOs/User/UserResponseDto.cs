using GrowAndGlow.Api.Models;

namespace GrowAndGlow.Api.DTOs.User
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ZodiacSign ZodiacSign { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
