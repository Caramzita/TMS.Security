using TMS.Security.Core;

namespace TMS.Security.UseCases.Abstractions;

public interface ITokenService
{
    string GenerateAccessToken(User user);

    Task<RefreshToken> GenerateRefreshToken(Guid userId);

    Task<RefreshToken?> DeactivateRefreshToken(string token);
}
