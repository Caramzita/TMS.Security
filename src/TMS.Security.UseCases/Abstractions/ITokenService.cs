using TMS.Security.Core;

namespace TMS.Security.UseCases.Abstractions;

/// <summary>
/// Сервис для работы с токенами, включая генерацию и деактивацию токенов доступа и обновления.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Генерирует токен доступа для указанного пользователя.
    /// </summary>
    /// <param name="user"> Пользователь, для которого генерируется токен. </param>
    /// <returns> Сгенерированный токен доступа в виде строки. </returns>
    /// <exception cref="ArgumentNullException"> Выбрасывается, если <paramref name="user"/> равен null. </exception>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Генерирует новый токен обновления для указанного пользователя.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя, для которого генерируется токен. </param>
    Task<RefreshToken> GenerateRefreshToken(Guid userId);

    /// <summary>
    /// Деактивирует токен обновления, если он действителен.
    /// </summary>
    /// <param name="token"> Токен, который нужно деактивировать. </param>
    /// <returns> Деактивированный токен обновления, или null, если токен не найден или не действителен. </returns>
    Task<RefreshToken?> DeactivateRefreshToken(string token);
}
