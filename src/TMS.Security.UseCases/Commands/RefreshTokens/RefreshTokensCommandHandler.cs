using MediatR;
using TMS.Security.UseCases.Abstractions;
using TMS.Security.UseCases.Commands.Login;
using TMS.Security.UseCases.Services;

namespace TMS.Security.UseCases.Commands.RefreshTokens;

public class RefreshTokensCommandHandler : LoginBaseHandler, IRequestHandler<RefreshTokensCommand>
{
    private readonly IUserRepository _userRepository;

    public RefreshTokensCommandHandler(IUserRepository userRepository, 
        ITokenService tokenService) : base(tokenService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _tokenService.DeactivateRefreshToken(request.RefreshToken);

        if (refreshToken is null)
        {
            return;
        }

        var user = await _userRepository.GetByIdAsync(refreshToken.UserId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        return await ContinueLoginHandle(user);
    }
}
