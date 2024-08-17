using TMS.Security.Core;

namespace TMS.Security.UseCases.Abstractions;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetByTokenAsync(string token);

    Task CreateAsync(RefreshToken token);

    Task DeactivateAsync(RefreshToken token);

    Task DeactivateAllTokensAsync(Guid userId);
}
