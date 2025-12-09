using GrowAndGlow.Api.Models;

namespace GrowAndGlow.Api.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(User user);
        RefreshToken CreateRefreshToken(int userId);
    }
}