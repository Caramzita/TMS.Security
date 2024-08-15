using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace TMS.Security.Integration;

public static class SwaggerHelper
{
    public static IServiceCollection AddSwagger(this IServiceCollection service, string xmlFilePath)
    {
        service.AddSwaggerGen(opts =>
        {
            opts.IncludeXmlComments(xmlFilePath);

            opts.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = @"Enter access token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            opts.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme,
                        },
                    },
                    new List<string>()
                }
            });
        });

        return service;
    }
}
