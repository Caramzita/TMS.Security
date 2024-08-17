using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Security.DataAccess.Dto;

namespace TMS.Security.DataAccess.Configurations;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshTokenDto>
{
    public void Configure(EntityTypeBuilder<RefreshTokenDto> builder)
    {
        builder.HasKey(token => token.Id);

        builder.HasOne<UserDto>()
            .WithMany()
            .HasForeignKey(token => token.UserId);
    }
}
