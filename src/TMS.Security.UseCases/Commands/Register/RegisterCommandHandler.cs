using MediatR;
using TMS.Application.UseCases;
using TMS.Security.Contracts;
using TMS.Security.Core;
using TMS.Security.UseCases.Abstractions;
using TMS.Security.UseCases.Commands.Login;

namespace TMS.Security.UseCases.Commands.Register;

public class RegisterCommandHandler : LoginBaseHandler, IRequestHandler<RegisterCommand, Result<Tokens>>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository, ITokenService tokenService)
        : base(tokenService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<Result<Tokens>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByUsernameAsync(request.Username) != null)
        {
            return Result<Tokens>.Invalid("User with this login already exists");
        }

        if (await _userRepository.ExistsByEmailAsync(request.Email))
        {
            return Result<Tokens>.Invalid("User with this email already exists");
        }

        var user = new User(request.Username, request.Password, request.Email);

        await _userRepository.CreateAsync(user);

        return await ContinueLoginHandle(user);
    }
}
