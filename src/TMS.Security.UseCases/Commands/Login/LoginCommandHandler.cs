using MediatR;
using TMS.Application.UseCases;
using TMS.Security.Contracts;
using TMS.Security.Core.Services;
using TMS.Security.UseCases.Abstractions;

namespace TMS.Security.UseCases.Commands.Login;

/// <summary>
/// Обработчик команды для аутентификации пользователя.
/// </summary>
public class LoginCommandHandler : LoginBaseHandler, IRequestHandler<LoginCommand, Result<Tokens>>
{
    /// <summary>
    /// Репозиторий для доступа к пользователям.
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LoginCommandHandler"/>.
    /// </summary>
    /// <param name="userRepository"> Репозиторий для доступа к данным пользователя. </param>
    /// <param name="tokenService"> Сервис для работы с токенами. </param>
    /// <exception cref="ArgumentNullException"> Бросается, если <paramref name="userRepository"/> 
    /// или <paramref name="tokenService"/> равен <c>null</c>. </exception>
    public LoginCommandHandler(IUserRepository userRepository,
                               ITokenService tokenService) : base(tokenService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Обрабатывает команду входа пользователя в систему.
    /// </summary>
    /// <param name="request"> Команда входа с именем пользователя и паролем. </param>
    /// <param name="cancellationToken"> Токен отмены задачи. </param>
    /// <returns> Результат выполнения команды с токенами доступа. </returns>
    public async Task<Result<Tokens>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user == null)
        {
            return Result<Tokens>.Invalid("Invalid password or login");
        }

        if (!CryptographyService.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Result<Tokens>.Invalid("Incorrect password");
        }

        return await ContinueLoginHandle(user);
    }
}
