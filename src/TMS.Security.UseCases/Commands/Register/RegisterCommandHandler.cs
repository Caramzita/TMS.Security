using MediatR;
using Microsoft.AspNetCore.Identity;

namespace TMS.Security.UseCases.Commands.Registration;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IdentityResult>
{
    private readonly UserManager<IdentityUser> _userManager;

    public RegisterCommandHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
        {
            var error = new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "Email is already taken."
            };
            return IdentityResult.Failed(error);
        }

        var user = new IdentityUser { UserName = request.Username, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        return result;
    }
}
