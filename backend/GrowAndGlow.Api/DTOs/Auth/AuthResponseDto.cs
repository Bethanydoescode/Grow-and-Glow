namespace GrowAndGlow.Api.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ZodiacSign { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}

//➡ This matches the API contract return shape.