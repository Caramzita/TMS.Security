using MediatR;
using TMS.Application.UseCases;
using TMS.Security.Contracts;
using TMS.Security.Core;
using TMS.Security.UseCases.Abstractions;
using TMS.Security.UseCases.Commands.Login;

namespace TMS.Security.UseCases.Commands.Register;

/// <summary>
/// Обработчик команды регистрации пользователя. Обрабатывает запрос на регистрацию нового пользователя,
/// проверяет наличие существующих учетных записей и создает новую учетную запись пользователя.
/// </summary>
public class RegisterCommandHandler : LoginBaseHandler, IRequestHandler<RegisterCommand, Result<Tokens>>
{
    /// <summary>
    /// Репозиторий для доступа к пользователям.
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RegisterCommandHandler"/>.
    /// </summary>
    /// <param name="userRepository"> Репозиторий пользователей для взаимодействия с базой данных пользователей. </param>
    /// <param name="tokenService"> Сервис для генерации и управления токенами. </param>
    /// <exception cref="ArgumentNullException"> Возникает, если <paramref name="userRepository"/> 
    /// или <paramref name="tokenService"/> равны <c>null</c>. </exception>
    public RegisterCommandHandler(IUserRepository userRepository, ITokenService tokenService)
        : base(tokenService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Обрабатывает команду регистрации пользователя. Проверяет уникальность имени пользователя и электронной почты,
    /// создаёт нового пользователя и возвращает результат с токенами, если регистрация прошла успешно.
    /// </summary>
    /// <param name="request"> Команда регистрации пользователя, содержащая имя пользователя, пароль и электронную почту. </param>
    /// <param name="cancellationToken"> Токен отмены операции. </param>
    /// <returns> Результат регистрации с токенами, если регистрация успешна, иначе сообщение об ошибке. </returns>
    public async Task<Result<Tokens>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByUsernameAsync(request.Username) != null)
        {
            return Result<Tokens>.Invalid("User with this login already exists");
        }

        if (await _userRepository.ExistsByEmailAsync(request.Email))
        {
            return Result<Tokens>.Invalid("User with this email already exists");
        }

        var user = new User(request.Username, request.Password, request.Email);

        await _userRepository.CreateAsync(user);

        return await ContinueLoginHandle(user);
    }
}
