using MediatR;
using TMS.Application.UseCases;
using TMS.Security.Contracts;
using TMS.Security.Core.Services;
using TMS.Security.UseCases.Abstractions;
using TMS.Security.UseCases.Commands.Login;

namespace TMS.Security.UseCases.Commands.ChangePassword;

/// <summary>
/// Обработчик команды изменения пароля пользователя.
/// </summary>
public class ChangePasswordCommandHandler : LoginBaseHandler, IRequestHandler<ChangePasswordCommand, Result<Tokens>>
{
    /// <summary>
    /// Репозиторий для доступа к пользователям.
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Репозиторий для доступа к токенам обновления.
    /// </summary>
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ChangePasswordCommandHandler"/>.
    /// </summary>
    /// <param name="userRepository"> Репозиторий для работы с пользователями. </param>
    /// <param name="refreshTokenRepository">Репозиторий для работы с refresh токенами.</param>
    /// <param name="tokenService">Сервис для работы с токенами.</param>
    /// <exception cref="ArgumentNullException"> Бросается, если <paramref name="userRepository"/>, 
    /// <paramref name="refreshTokenRepository"/> или 
    /// <paramref name="tokenService"/> равны <c>null</c>. </exception>
    public ChangePasswordCommandHandler(IUserRepository userRepository, 
        IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService) : base(tokenService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _refreshTokenRepository = 
            refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
    }

    /// <summary>
    /// Обрабатывает команду изменения пароля пользователя.
    /// </summary>
    /// <param name="command"> Команда для изменения пароля. </param>
    /// <param name="cancellationToken"> Токен отмены операции. </param>
    /// <returns> Результат выполнения команды с токенами, если операция успешна; иначе сообщение об ошибке. </returns>
    public async Task<Result<Tokens>> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(command.Username);

        if (user == null)
        {
            return Result<Tokens>.Invalid("User not found.");
        }

        if (!CryptographyService.VerifyPassword(command.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Result<Tokens>.Invalid("Invalid password or login");
        }

        user.SetPassword(command.NewPassword);

        await _refreshTokenRepository.DeactivateAllTokensAsync(user.Id);
        await _userRepository.UpdateAsync(user);

        return await ContinueLoginHandle(user);
    }
}
