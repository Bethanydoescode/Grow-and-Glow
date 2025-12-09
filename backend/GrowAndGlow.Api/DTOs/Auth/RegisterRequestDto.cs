using System.ComponentModel.DataAnnotations;
using GrowAndGlow.Api.Models;
using System.Text.Json.Serialization;


namespace GrowAndGlow.Api.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
       [JsonConverter(typeof(JsonStringEnumConverter))]
public ZodiacSign ZodiacSign { get; set; }

    }
}
