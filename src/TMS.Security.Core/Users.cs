using System.Security.Claims;
using TMS.Security.Core.Services;

namespace TMS.Security.Core;

/// <summary>
/// Представляет пользователя системы с данными для аутентификации и авторизации.
/// </summary>
public class User
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Хэш пароля пользователя.
    /// </summary>
    public byte[] PasswordHash { get; private set; }

    /// <summary>
    /// Соль, использованная для хэширования пароля пользователя.
    /// </summary>
    public byte[] PasswordSalt { get; private set; }

    /// <summary>
    /// Электронная почта пользователя.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="User"/> с указанным именем пользователя, паролем и электронной почтой.
    /// </summary>
    /// <param name="username"> Имя пользователя. </param>
    /// <param name="password"> Пароль пользователя. </param>
    /// <param name="email"> Электронная почта пользователя. </param>
    public User(string username, string password, string email)
    {
        Id = Guid.NewGuid();
        Username = username;
        SetPassword(password);
        Email = email;
    }

    /// <summary>
    /// Конструктор для инициализации модели уже существующего пользователя.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <param name="username"> Имя пользователя. </param>
    /// <param name="passwordHash"> Хэш пароля. </param>
    /// <param name="passwordSalt"> Соль пароля. </param>
    /// <param name="email"> Адрес электронной почты. </param>
    public User(Guid id, string username, byte[] passwordHash, byte[] passwordSalt, string email)
    {
        Id = id;
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        Email = email;
    }

    /// <summary>
    /// Устанавливает хэш пароля пользователя и соль.
    /// </summary>
    /// <param name="password">Пароль пользователя.</param>
    public void SetPassword(string password)
    {
        (PasswordHash, PasswordSalt) = CryptographyService.CreatePasswordHash(password);
    }

    /// <summary>
    /// Получает массив утверждений (claims) для аутентификации пользователя.
    /// </summary>
    /// <returns>Массив утверждений (claims) пользователя.</returns>
    public Claim[] GetClaims()
    {
        return
        [
            new(ClaimTypes.NameIdentifier, Id.ToString()),
            new(ClaimTypes.Name, Username),
            new(ClaimTypes.Email, Email)
        ];
    }
}
