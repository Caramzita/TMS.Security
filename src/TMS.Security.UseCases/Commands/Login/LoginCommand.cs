using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TMS.Security.UseCases.Commands.Login;

public class LoginCommand : IRequest<string>
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
