using AutoMapper;
using TMS.Security.Core;
using TMS.Security.DataAccess.Dto;

namespace TMS.Security.DataAccess;

/// <summary>
/// Профиль маппинга для AutoMapper, определяющий преобразования между сущностями домена и DTO.
/// </summary>
public class RepositoryMappingProfile : Profile
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RepositoryMappingProfile"/> и задает конфигурации маппинга.
    /// </summary>
    public RepositoryMappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();

        CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
    }
}
