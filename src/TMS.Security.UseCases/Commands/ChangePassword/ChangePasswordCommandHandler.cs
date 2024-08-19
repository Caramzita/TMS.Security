using MediatR;
using TMS.Application.UseCases;
using TMS.Security.Contracts;
using TMS.Security.Core.Services;
using TMS.Security.UseCases.Abstractions;
using TMS.Security.UseCases.Commands.Login;

namespace TMS.Security.UseCases.Commands.ChangePassword;

public class ChangePasswordCommandHandler : LoginBaseHandler, IRequestHandler<ChangePasswordCommand, Result<Tokens>>
{
    private readonly IUserRepository _userRepository;

    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public ChangePasswordCommandHandler(IUserRepository userRepository, 
        IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService) : base(tokenService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _refreshTokenRepository = 
            refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
    }

    public async Task<Result<Tokens>> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(command.Login);

        if (user == null)
        {
            return Result<Tokens>.Invalid("User not found.");
        }

        if (!CryptographyService.VerifyPassword(command.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Result<Tokens>.Invalid("Invalid password or login");
        }

        user.SetPassword(command.NewPassword);

        await _refreshTokenRepository.DeactivateAllTokensAsync(user.Id);
        await _userRepository.UpdateAsync(user);

        return await ContinueLoginHandle(user);
    }
}
