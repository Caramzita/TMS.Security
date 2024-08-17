using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TMS.Security.UseCases.Commands.Login;

public class LoginCommand : IRequest<string>
{
    [Required]
    public string Email { get; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; } = string.Empty;

    public LoginCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
