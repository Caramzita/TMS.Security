using TMS.Security.DataAccess;
using TMS.Security.UseCases.Abstractions;
using System.Reflection;
using TMS.Security.DataAccess.Repositories;
using TMS.Security.UseCases.Commands.Register;
using TMS.Security.Service.Infrastructure;
using Serilog.Events;
using Serilog;
using MediatR;
using TMS.Application.UseCases.DI;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using TMS.Application.Consul;
using TMS.Application.Security;
using Microsoft.Extensions.Options;

/// <summary>
/// Ёкземпл€р класса <see cref="Program"/>.
/// </summary>
public class Program
{
    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .WriteTo.Async(a => a.Console())
        .WriteTo.Async(a => a.File("logs/AuthWebAppLog.txt", rollingInterval: RollingInterval.Day))
        .CreateLogger();

        try
        {
            Log.Information("Starting up the application");
            var builder = ConfigureApp(args);
            await RunApp(builder);
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "An error occurred while app initialization");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static WebApplicationBuilder ConfigureApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Host.UseSerilog();

        var services = builder.Services;

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks();

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        services.AddSwagger(xmlFilePath);

        services.AddHttpContextAccessor();

        services.AddConsul(builder.Configuration);

        services.AddJwtSettingsFromConsul(builder.Configuration["ConsulKey"]);

        services.AddJwtBearerAuthentication();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        ConfigureDI(services, builder.Configuration);

        return builder;
    }

    private static void ConfigureDI(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<DataBaseContext>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RegisterCommand).Assembly));
        services.AddAutoMapper(typeof(RepositoryMappingProfile).Assembly, typeof(ControllerMappingProfile).Assembly);

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    private static async Task RunApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseCors();

        app.UseMiddleware<ErrorExceptionHandler>();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        //using (var scope = app.Services.CreateScope())
        //{
        //    var db = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
        //    db.Database.Migrate();
        //}

        await app.RunAsync();
    }
}