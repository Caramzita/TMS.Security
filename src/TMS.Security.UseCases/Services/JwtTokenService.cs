using TMS.Security.UseCases.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace TMS.Security.UseCases.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string GenerateToken(IdentityUser user)
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
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            }),
            Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(60)),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
