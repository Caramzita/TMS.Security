using MediatR;
using TMS.Application.UseCases;
using TMS.Security.Contracts;
using TMS.Security.UseCases.Abstractions;
using TMS.Security.UseCases.Commands.Login;
using TMS.Security.UseCases.Services;

namespace TMS.Security.UseCases.Commands.RefreshTokens;

public class RefreshTokensCommandHandler : LoginBaseHandler, IRequestHandler<RefreshTokensCommand, Result<Tokens>>
{
    private readonly IUserRepository _userRepository;

    public RefreshTokensCommandHandler(IUserRepository userRepository, 
        ITokenService tokenService) : base(tokenService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Result<Tokens>> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _tokenService.DeactivateRefreshToken(request.RefreshToken);

        if (refreshToken is null)
        {
            return Result<Tokens>.Invalid("Refresh token is incorrect");
        }

        var user = await _userRepository.GetByIdAsync(refreshToken.UserId);

        if (user == null)
        {
            return Result<Tokens>.Invalid("User not found.");
        }

        return await ContinueLoginHandle(user);
    }
}
