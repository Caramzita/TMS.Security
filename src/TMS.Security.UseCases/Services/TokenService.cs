using TMS.Security.UseCases.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using TMS.Security.Core;

namespace TMS.Security.UseCases.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
    }

    public string GenerateAccessToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var key = Encoding.UTF8.GetBytes("K17T6p+mYlBuIll6EOQDUmAdM6xmzeHOpE+O35zsAvw=");
        //var issuer = _configuration["JwtSecurityToken:Issuer"];
        //var audience = _configuration["JwtSecurityToken:Audience"];
        //var expiresInMinutes = TimeSpan.FromMinutes(double.Parse(_configuration["JwtSecurityToken:Expires"]!));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            //Issuer = issuer,
            //Audience = audience,
            //Subject = new ClaimsIdentity(new[]
            //{
            //    new Claim(ClaimTypes.NameIdentifier, user.Id),
            //    new Claim(ClaimTypes.Name, user.UserName!),
            //    new Claim(ClaimTypes.Email, user.Email!)
            //}),
            Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<RefreshToken> GenerateRefreshToken(Guid userId)
    {
        var expiration = DateTime.UtcNow.AddDays(20);
        var token = new RefreshToken(expiration, userId);

        await _refreshTokenRepository.CreateAsync(token);

        return token;
    }

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
