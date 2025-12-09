using GrowAndGlow.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace GrowAndGlow.Api.DTOs.User
{
    public class UpdateProfileRequestDto
    {
        [MaxLength(50)]
        public string? DisplayName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public ZodiacSign? ZodiacSign { get; set; }

        // Password change
        public string? CurrentPassword { get; set; }

        [MinLength(6)]
        public string? NewPassword { get; set; }
    }
}
