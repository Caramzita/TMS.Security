using TMS.Security.Core;

namespace TMS.Security.UseCases.Abstractions;

/// <summary>
/// Репозиторий для доступа к токенам обновления.
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Получает токен обновления по его значению.
    /// </summary>
    /// <param name="token"> Значение токена обновления.</param>
    /// <returns>Экземпляр <see cref="RefreshToken"/> или null, если токен не найден. </returns>
    Task<RefreshToken> GetByTokenAsync(string token);

    /// <summary>
    /// Создает новый токен обновления и сохраняет его в базе данных.
    /// </summary>
    /// <param name="token"> Экземпляр <see cref="RefreshToken"/> для сохранения. </param>
    Task CreateAsync(RefreshToken token);

    /// <summary>
    /// Деактивирует токен обновления и обновляет его в базе данных.
    /// </summary>
    /// <param name="token"> Токен обновления для деактивации. </param>
    Task DeactivateAsync(RefreshToken token);

    /// <summary>
    /// Деактивирует все токены обновления для указанного пользователя.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя, для которого деактивируются все токены. </param>
    Task DeactivateAllTokensAsync(Guid userId);
}
