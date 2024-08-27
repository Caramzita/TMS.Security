namespace TMS.Security.DataAccess.Dto;

/// <summary>
/// Представляет DTO для пользователя.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Хэш пароля пользователя.
    /// </summary>
    public byte[] PasswordHash { get; set; }

    /// <summary>
    /// Соль, использованная для хэширования пароля пользователя.
    /// </summary>
    public byte[] PasswordSalt { get; set; }

    /// <summary>
    /// Адрес электронной почты пользователя.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}