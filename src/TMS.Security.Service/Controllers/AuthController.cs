using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.UseCases;
using TMS.Security.Contracts;
using TMS.Security.Contracts.Requests;
using TMS.Security.UseCases.Commands.ChangePassword;
using TMS.Security.UseCases.Commands.Login;
using TMS.Security.UseCases.Commands.RefreshTokens;
using TMS.Security.UseCases.Commands.Register;

namespace TMS.Security.Service.Controllers;

/// <summary>
/// Предоставляет Rest API для работы с авторизацией пользователей
/// </summary>
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    /// <summary>
    /// Медиатр.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Маппер.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AuthController"/>.
    /// </summary>
    /// <param name="mediator">Интерфейс для отправки команд и запросов через Mediator.</param>
    /// <param name="mapper">Интерфейс для маппинга данных между моделями.</param>
    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Регистрация пользователя.
    /// </summary>
    /// <param name="request"> Данные пользователя </param>
    /// <response code="200"> Успешно </response>
    /// <response code="400"> Переданные параметры не прошли валидацию </response>
    /// <response code="409"> Пользователь с переданным логином уже существует </response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(Tokens), 201)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Авторизация пользователя.
    /// </summary>
    /// <returns> Токен доступа и токен обновления. </returns>
    /// <param name="request"> Данные пользователя </param>
    /// <response code="200"> Успешно </response>
    /// <response code="400"> Переданные параметры не прошли валидацию </response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(Tokens), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = _mapper.Map<LoginCommand>(request);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Изменение пароля пользователя.
    /// </summary>
    /// <param name="request"> JSON объект, содержащий логин/старый пароль/новый пароль </param>
    /// <response code="200"> Успешно </response>
    /// <response code="400"> Переданные параметры не прошли валидацию </response>
    [HttpPut("changePassword")]
    [ProducesResponseType(typeof(Tokens), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var command = _mapper.Map<ChangePasswordCommand>(request);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Получить новую пару токенов по токену обновления.
    /// </summary>
    /// <param name="request"> Запрос на обновление токенов авторизации. </param>
    /// <response code="200"> Успешно </response>
    /// <response code="400"> Переданные параметры не прошли валидацию </response>
    [HttpPost("refreshTokens")]
    [ProducesResponseType(typeof(Tokens), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var command = _mapper.Map<RefreshTokensCommand>(request);
        var result = await _mediator.Send(command);

        return result.ToActionResult();
    }
}
