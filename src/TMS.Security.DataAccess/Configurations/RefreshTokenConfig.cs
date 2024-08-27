using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Security.DataAccess.Dto;

namespace TMS.Security.DataAccess.Configurations;

/// <summary>
/// Конфигурирует сущность <see cref="RefreshTokenDto"/> в Entity Framework Core.
/// </summary>
public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshTokenDto>
{
    /// <summary>
    /// Выполняет настройку конфигурации для сущности <see cref="RefreshTokenDto"/>.
    /// </summary>
    /// <param name="builder">Строитель конфигурации сущности <see cref="RefreshTokenDto"/>.</param>
    public void Configure(EntityTypeBuilder<RefreshTokenDto> builder)
    {
        builder.HasKey(token => token.Id);

        builder.HasOne<UserDto>()
            .WithMany()
            .HasForeignKey(token => token.UserId);
    }
}
