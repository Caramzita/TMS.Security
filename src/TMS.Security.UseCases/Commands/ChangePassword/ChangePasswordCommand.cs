using MediatR;
using TMS.Security.Contracts;
using TMS.Application.UseCases;

namespace TMS.Security.UseCases.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<Result<Tokens>>
{
    public string Login {  get; }

    public string Password { get; }

    public string NewPassword { get; }

    public ChangePasswordCommand(string login, string password, string newPassword)
    {
        Login = login;
        Password = password;
        NewPassword = newPassword;
    }
}
