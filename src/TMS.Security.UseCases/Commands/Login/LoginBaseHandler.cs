using TMS.Security.Core;
using TMS.Security.UseCases.Abstractions;

namespace TMS.Security.UseCases.Commands.Login;

public abstract class LoginBaseHandler
{
    public readonly ITokenService _tokenService;

    public LoginBaseHandler(ITokenService tokenService)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    public async Task<Result<Tokens>> ContinueLoginHandle(User user)
    {
        string accessToken = _tokenService.GenerateAccessToken(user);
        RefreshToken refreshToken = await _tokenService.GenerateRefreshToken(user.Id);

        return Result<Tokens>.Success(
            new Tokens(accessToken, refreshToken.Token, refreshToken.Expiration));
    }
}
