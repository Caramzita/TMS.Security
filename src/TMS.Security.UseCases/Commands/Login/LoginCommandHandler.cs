using MediatR;
using Microsoft.AspNetCore.Identity;
using TMS.Security.UseCases.Abstractions;

namespace TMS.Security.UseCases.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    //private readonly 
    private readonly UserManager<IdentityUser> _userManager;

    private readonly SignInManager<IdentityUser> _signInManager;

    private readonly ITokenService _jwtTokenService;

    public LoginCommandHandler(UserManager<IdentityUser> userManager,
                               SignInManager<IdentityUser> signInManager,
                               ITokenService jwtTokenService)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email) 
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        return _jwtTokenService.GenerateToken(user);
    }
}
