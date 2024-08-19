using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.UseCases;
using TMS.Security.Contracts.Requests;
using TMS.Security.UseCases.Commands.ChangePassword;
using TMS.Security.UseCases.Commands.Login;
using TMS.Security.UseCases.Commands.RefreshTokens;
using TMS.Security.UseCases.Commands.Register;

namespace TMS.Security.Service.Controllers;

[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = _mapper.Map<LoginCommand>(request);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    [HttpPut("changePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var command = _mapper.Map<ChangePasswordCommand>(request);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    [HttpPost("refreshTokens")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var command = _mapper.Map<RefreshTokensCommand>(request);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }
}
