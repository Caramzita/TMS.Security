using MediatR;
using TMS.Application.UseCases;
using TMS.Security.Contracts;

namespace TMS.Security.UseCases.Commands.RefreshTokens;

/// <summary>
/// Команда для обновления токенов на основе предоставленного токена обновления.
/// </summary>
public class RefreshTokensCommand : IRequest<Result<Tokens>>
{
    /// <summary>
    /// Получает токен обновления, используемый для запроса нового токена доступа и токена обновления.
    /// </summary>
    public string RefreshToken { get; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RefreshTokensCommand"/>.
    /// </summary>
    /// <param name="refreshToken"> Токен обновления, используемый для обновления токенов. </param>
    public RefreshTokensCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
