using Microsoft.AspNetCore.Identity;

namespace TMS.Security.UseCases.Abstractions;

public interface IJwtTokenService
{
    string GenerateToken(IdentityUser user);
}
