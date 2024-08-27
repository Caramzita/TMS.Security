using FluentValidation;

namespace TMS.Security.UseCases.Commands.Login;

/// <summary>
/// Валидатор для команды входа пользователя.
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LoginCommandValidator"/>.
    /// </summary>
    public LoginCommandValidator()
    {
        RuleFor(command => command.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(20).WithMessage("Username must not exceed 20 characters.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.");

        RuleFor(command => command.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(4).WithMessage("Password must be at least 4 characters long.");
    }
}
