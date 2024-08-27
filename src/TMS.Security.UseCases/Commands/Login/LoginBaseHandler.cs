using TMS.Application.UseCases;
using TMS.Security.Contracts;
using TMS.Security.Core;
using TMS.Security.UseCases.Abstractions;

namespace TMS.Security.UseCases.Commands.Login;

/// <summary>
/// Базовый обработчик авторизации.
/// </summary>
public abstract class LoginBaseHandler
{
    /// <summary>
    /// Сервис для работы с токенами.
    /// </summary>
    public readonly ITokenService _tokenService;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LoginBaseHandler"/>.
    /// </summary>
    /// <param name="tokenService"> Сервис для работы с токенами. </param>
    /// <exception cref="ArgumentNullException"> Бросается, если <paramref name="tokenService"/> равен <c>null</c>. </exception>
    public LoginBaseHandler(ITokenService tokenService)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    /// <summary>
    /// Продолжает обработку входа, создавая новые токены и возвращая результат.
    /// </summary>
    /// <param name="user"> Пользователь, для которого создаются токены. </param>
    /// <returns> Результат выполнения с новыми токенами. </returns>
    public async Task<Result<Tokens>> ContinueLoginHandle(User user)
    {
        string accessToken = _tokenService.GenerateAccessToken(user);
        RefreshToken refreshToken = await _tokenService.GenerateRefreshToken(user.Id);

        return Result<Tokens>.Success(
            new Tokens(accessToken, refreshToken.Token, refreshToken.Expires));
    }
}
