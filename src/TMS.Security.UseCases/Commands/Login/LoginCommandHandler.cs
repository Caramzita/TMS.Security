using MediatR;
using TMS.Application.UseCases;
using TMS.Security.Contracts;
using TMS.Security.Core.Services;
using TMS.Security.UseCases.Abstractions;

namespace TMS.Security.UseCases.Commands.Login;

public class LoginCommandHandler : LoginBaseHandler, IRequestHandler<LoginCommand, Result<Tokens>>
{
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(IUserRepository userRepository,
                               ITokenService tokenService) : base(tokenService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Result<Tokens>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user == null)
        {
            return Result<Tokens>.Invalid("Invalid password or login");
        }

        if (!CryptographyService.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Result<Tokens>.Invalid("Incorrect password");
        }

        return await ContinueLoginHandle(user);
    }
}
