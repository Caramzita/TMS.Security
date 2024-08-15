using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TMS.Security.Integration;

public static class AuthenticationHelper
{
    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                //ValidAudience = configuration["JwtSecurityToken:Audience"],
                //ValidIssuer = configuration["JwtSecurityToken:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes("K17T6p+mYlBuIll6EOQDUmAdM6xmzeHOpE+O35zsAvw=")),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };
        });

        return services;
    }
}
