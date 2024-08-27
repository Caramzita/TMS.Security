using MediatR;
using System.ComponentModel.DataAnnotations;
using TMS.Application.UseCases;
using TMS.Security.Contracts;

namespace TMS.Security.UseCases.Commands.Login;

/// <summary>
/// Команда для обработки запроса на вход пользователя в систему.
/// </summary>
public class LoginCommand : IRequest<Result<Tokens>>
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
    /// Инициализирует новый экземпляр <see cref="LoginCommand"/>.
    /// </summary>
    /// <param name="username"> Имя пользователя для входа. </param>
    /// <param name="password"> Пароль пользователя для входа. </param>
    public LoginCommand(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
