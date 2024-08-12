using MediatR;
using Microsoft.AspNetCore.Identity;
using TMS.Security.UseCases.Abstractions;

namespace TMS.Security.UseCases.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly UserManager<IdentityUser> _userManager;

    private readonly SignInManager<IdentityUser> _signInManager;

    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(UserManager<IdentityUser> userManager,
                               SignInManager<IdentityUser> signInManager,
                               IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null) 
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded) 
            return null;

        return _jwtTokenService.GenerateToken(user);
    }
}
