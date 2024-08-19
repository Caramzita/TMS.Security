using MediatR;
using System.ComponentModel.DataAnnotations;
using TMS.Application.UseCases;
using TMS.Security.Contracts;

namespace TMS.Security.UseCases.Commands.Login;

public class LoginCommand : IRequest<Result<Tokens>>
{
    [Required]
    public string Username { get; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; } = string.Empty;

    public LoginCommand(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
