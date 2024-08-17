using TMS.Security.Core;

namespace TMS.Security.UseCases.Abstractions;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);

    Task<User> GetByUsernameAsync(string username);

    Task<bool> ExistsByEmailAsync(string email);

    Task<User> CreateAsync(User user);

    Task UpdateAsync(User user);

    Task DeleteAsync(User user);
}
