using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Security.DataAccess.Dto;

namespace TMS.Security.DataAccess.Configurations;

/// <summary>
/// Конфигурирует сущность <see cref="UserDto"/> в Entity Framework Core.
/// </summary>
public class UserConfig : IEntityTypeConfiguration<UserDto>
{
    /// <summary>
    /// Выполняет настройку конфигурации для сущности <see cref="UserDto"/>.
    /// </summary>
    /// <param name="builder">Строитель конфигурации сущности <see cref="UserDto"/>.</param>
    public void Configure(EntityTypeBuilder<UserDto> builder)
    {
        builder.HasKey(user => user.Id);
        builder.HasAlternateKey(user => user.Username);

        builder.HasIndex(user => user.Email).IsUnique();
    }
}
