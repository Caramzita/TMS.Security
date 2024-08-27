using MediatR;
using System.ComponentModel.DataAnnotations;
using TMS.Application.UseCases;
using TMS.Security.Contracts;

namespace TMS.Security.UseCases.Commands.Register;

/// <summary>
/// Команда для регистрации нового пользователя с предоставлением данных для создания учетной записи.
/// </summary>
public class RegisterCommand : IRequest<Result<Tokens>>
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    [Required]
    public string Username { get; } = string.Empty;

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; } = string.Empty;

    /// <summary>
    /// Адрес электронной почты пользователя.
    /// </summary>
    [Required]
    public string Email { get; } = string.Empty;

    // <summary>
    /// Инициализирует новый экземпляр <see cref="RegisterCommand"/>.
    /// </summary>
    /// <param name="username"> Имя пользователя. </param>
    /// <param name="password"> Пароль пользователя. </param>
    /// <param name="email"> Адрес электронной почты пользователя. </param>
    public RegisterCommand(string username, string password, string email)
    {
        Username = username;
        Password = password;
        Email = email;
    }
}
