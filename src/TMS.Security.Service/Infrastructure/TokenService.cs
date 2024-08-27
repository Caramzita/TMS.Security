using TMS.Security.UseCases.Abstractions;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using TMS.Security.Core;
using Microsoft.Extensions.Options;
using TMS.Application.Security;

namespace TMS.Security.Service.Infrastructure;

/// <summary>
/// Реализация <see cref="ITokenService"/>.
/// </summary>
public class TokenService : ITokenService
{
    /// <summary>
    /// Репозиторий для доступа к токенам обновления.
    /// </summary>
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    /// <summary>
    /// Параметры jwt токена.
    /// </summary>
    private readonly JwtTokenSettings _jwtSettings;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TokenService"/>.
    /// </summary>
    /// <param name="refreshTokenRepository"> Репозиторий для работы с токенами обновления. </param>
    /// <param name="jwtSettings"> Настройки JWT токенов. </param>
    /// <exception cref="ArgumentNullException"> Выбрасывается, если <paramref name="refreshTokenRepository"/> 
    /// или <paramref name="jwtSettings"/> равны null. </exception>
    public TokenService(IRefreshTokenRepository refreshTokenRepository, IOptions<JwtTokenSettings> jwtSettings)
    {
        _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
    }

    /// <inheritdoc/>
    public string GenerateAccessToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresIn = TimeSpan.FromMinutes(_jwtSettings.AccessTokenLifetimeInMinutes);

        var token = new JwtSecurityToken(
               issuer: _jwtSettings.Issuer,
               audience: _jwtSettings.Audience,
               claims: user.GetClaims(),
               notBefore: null,
               expires: DateTime.UtcNow.Add(expiresIn),
               signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }

    /// <inheritdoc/>
    public async Task<RefreshToken> GenerateRefreshToken(Guid userId)
    {
        var expiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeInMinutes);
        var token = new RefreshToken(expiration, userId);

        await _refreshTokenRepository.CreateAsync(token);

        return token;
    }

    /// <inheritdoc/>
    public async Task<RefreshToken?> DeactivateRefreshToken(string token)
    {
        var tokenModel = await _refreshTokenRepository.GetByTokenAsync(token);

        if (tokenModel is null || !tokenModel.IsValid())
        {
            return null;
        }

        await _refreshTokenRepository.DeactivateAsync(tokenModel);

        return tokenModel;
    }
}