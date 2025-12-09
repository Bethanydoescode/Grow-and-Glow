using GrowAndGlow.Api.Models;

namespace GrowAndGlow.Api.Repository.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> CreateTokenAsync(RefreshToken token);
        Task<RefreshToken?> GetValidTokenAsync(string token);
        Task RevokeTokenAsync(RefreshToken token);
    }
}
