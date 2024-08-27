using MediatR;
using TMS.Security.Contracts;
using TMS.Application.UseCases;

namespace TMS.Security.UseCases.Commands.ChangePassword;

/// <summary>
/// Команда для изменения пароля пользователя.
/// </summary>
public class ChangePasswordCommand : IRequest<Result<Tokens>>
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Текущий пароль пользователя.
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// Новый пароль.
    /// </summary>
    public string NewPassword { get; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ChangePasswordCommand"/>.
    /// </summary>
    /// <param name="username"> Имя пользователя, чей пароль нужно изменить. </param>
    /// <param name="password"> Текущий пароль пользователя. </param>
    /// <param name="newPassword">Н овый пароль, на который нужно изменить текущий. </param>
    public ChangePasswordCommand(string username, string password, string newPassword)
    {
        Username = username;
        Password = password;
        NewPassword = newPassword;
    }
}
