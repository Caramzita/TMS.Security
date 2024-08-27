using AutoMapper;
using TMS.Security.Contracts.Requests;
using TMS.Security.UseCases.Commands.ChangePassword;
using TMS.Security.UseCases.Commands.Login;
using TMS.Security.UseCases.Commands.RefreshTokens;
using TMS.Security.UseCases.Commands.Register;

namespace TMS.Security.Service.Infrastructure;

/// <summary>
/// Профиль AutoMapper для маппинга объектов запросов из контроллера на команды.
/// </summary>
public class ControllerMappingProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ControllerMappingProfile"/> и задает конфигурации маппинга.
    /// </summary>
    public ControllerMappingProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>()
            .ConstructUsing(request => new RegisterCommand
            (request.Username, request.Password, request.Email));

        CreateMap<LoginRequest, LoginCommand>()
            .ConstructUsing(request => new LoginCommand(request.Username, request.Password));

        CreateMap<ChangePasswordRequest, ChangePasswordCommand>()
            .ConstructUsing(request => new ChangePasswordCommand
            (request.Username, request.Password, request.NewPassword));

        CreateMap<RefreshTokenRequest, RefreshTokensCommand>()
            .ConstructUsing(request => new RefreshTokensCommand(request.RefreshToken));
    }
}
