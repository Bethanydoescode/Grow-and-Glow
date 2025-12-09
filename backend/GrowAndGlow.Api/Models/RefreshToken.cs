// --- Models/RefreshToken.cs ---
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrowAndGlow.Api.Models
{
    public class RefreshToken
    {
        [Key]
        public int TokenId { get; set; }

        [Required]
        public required string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        // Foreign Key
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
