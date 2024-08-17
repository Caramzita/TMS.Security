using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TMS.Security.UseCases.Commands.Registration;

public class RegisterCommand : IRequest<IdentityResult>
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Username { get; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Password { get; } = string.Empty;

    [Required]
    public string Email { get; } = string.Empty;

    public RegisterCommand(string username, string password, string email)
    {
        Username = username;
        Password = password;
        Email = email;
    }
}
