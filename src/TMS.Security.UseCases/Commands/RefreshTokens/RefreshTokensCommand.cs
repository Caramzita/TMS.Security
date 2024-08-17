using MediatR;

namespace TMS.Security.UseCases.Commands.RefreshTokens;

public class RefreshTokensCommand : IRequest
{
    public string RefreshToken { get; }

    public RefreshTokensCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
