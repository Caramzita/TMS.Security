using MediatR;
using TMS.Application.UseCases;
using TMS.Security.Contracts;

namespace TMS.Security.UseCases.Commands.RefreshTokens;

public class RefreshTokensCommand : IRequest<Result<Tokens>>
{
    public string RefreshToken { get; }

    public RefreshTokensCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
