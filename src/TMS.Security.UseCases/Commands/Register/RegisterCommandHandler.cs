using MediatR;
using Microsoft.AspNetCore.Identity;
using TMS.Security.Core;
using TMS.Security.UseCases.Abstractions;
using TMS.Security.UseCases.Commands.Login;

namespace TMS.Security.UseCases.Commands.Registration;

public class RegisterCommandHandler : LoginBaseHandler, IRequestHandler<RegisterCommand, IdentityResult>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<List<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.ExistsByEmailAsync(request.Email);

        if (!existingUser)
        {
            var error = new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "Email is already taken."
            };
            return IdentityResult.Failed(error);
        }

        var user = new User(request.Username, request.Password, request.Email);
        var result = await _userRepository.CreateAsync(user);

        return await ContinueLoginHandle(user);
    }
}
