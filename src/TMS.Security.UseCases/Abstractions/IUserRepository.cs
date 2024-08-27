using TMS.Security.Core;

namespace TMS.Security.UseCases.Abstractions;

/// <summary>
/// Репозиторий для доступа к пользователям.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Получает пользователя по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns> Экземпляр <see cref="User"/> или null, если пользователь не найден. </returns>
    Task<User> GetByIdAsync(Guid id);

    /// <summary>
    /// Получает пользователя по его имени пользователя (никнейму).
    /// </summary>
    /// <param name="username">Имя пользователя (никнейм).</param>
    /// <returns> Экземпляр <see cref="User"/> или null, если пользователь не найден. </returns>
    Task<User> GetByUsernameAsync(string username);

    /// <summary>
    /// Проверяет, существует ли пользователь с указанным адресом электронной почты.
    /// </summary>
    /// <param name="email"> Адрес электронной почты пользователя.</param>
    /// <returns>true, если пользователь с таким адресом электронной почты существует; иначе false. </returns>
    Task<bool> ExistsByEmailAsync(string email);

    /// <summary>
    /// Создает нового пользователя и сохраняет его в базе данных.
    /// </summary>
    /// <param name="user"> Экземпляр <see cref="User"/> для создания. </param>
    /// <returns> Созданный экземпляр <see cref="User"/>. </returns>
    Task<User> CreateAsync(User user);

    /// <summary>
    /// Обновляет информацию о пользователе в базе данных.
    /// </summary>
    /// <param name="user"> Экземпляр <see cref="User"/> для обновления. </param>
    Task UpdateAsync(User user);

    /// <summary>
    /// Удаляет пользователя из базы данных.
    /// </summary>
    /// <param name="user"> Экземпляр <see cref="User"/> для удаления. </param>
    Task DeleteAsync(User user);
}
