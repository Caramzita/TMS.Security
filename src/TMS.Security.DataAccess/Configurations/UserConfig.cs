using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Security.DataAccess.Dto;

namespace TMS.Security.DataAccess.Configurations;

public class UserConfig : IEntityTypeConfiguration<UserDto>
{
    public void Configure(EntityTypeBuilder<UserDto> builder)
    {
        builder.HasKey(user => user.Id);
        builder.HasAlternateKey(user => user.Username);

        builder.HasIndex(user => user.Email).IsUnique();
    }
}
