using MediatR;
using TMS.Application.UseCases;
using TMS.Security.Contracts;
using TMS.Security.UseCases.Abstractions;
using TMS.Security.UseCases.Commands.Login;

namespace TMS.Security.UseCases.Commands.RefreshTokens;

/// <summary>
/// Обработчик команды для обновления токенов, использующий токен обновления для генерации нового набора токенов.
/// </summary>
public class RefreshTokensCommandHandler : LoginBaseHandler, IRequestHandler<RefreshTokensCommand, Result<Tokens>>
{
    /// <summary>
    /// Репозиторий для доступа к пользователям.
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RefreshTokensCommandHandler"/>.
    /// </summary>
    /// <param name="userRepository"> Репозиторий пользователей, используемый для получения информации о пользователях. </param>
    /// <param name="tokenService"> Сервис для работы с токенами, включая генерацию и деактивацию. </param>
    public RefreshTokensCommandHandler(IUserRepository userRepository, 
        ITokenService tokenService) : base(tokenService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Обрабатывает команду обновления токенов, используя токен обновления для генерации нового набора токенов.
    /// </summary>
    /// <param name="request"> Команда с токеном обновления для обновления токенов. </param>
    /// <param name="cancellationToken"> Токен отмены задачи. </param>
    /// <returns> Результат выполнения команды, содержащий новый набор токенов или сообщение об ошибке. </returns>
    public async Task<Result<Tokens>> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _tokenService.DeactivateRefreshToken(request.RefreshToken);

        if (refreshToken is null)
        {
            return Result<Tokens>.Invalid("Refresh token is incorrect");
        }

        var user = await _userRepository.GetByIdAsync(refreshToken.UserId);

        if (user == null)
        {
            return Result<Tokens>.Invalid("User not found.");
        }

        return await ContinueLoginHandle(user);
    }
}
