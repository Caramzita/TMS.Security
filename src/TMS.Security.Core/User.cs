using TMS.Security.UseCases.Services;

namespace TMS.Security.Core;

public class User
{
    public Guid Id { get; private set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public User(string username, string password, string email)
    {
        Id = Guid.NewGuid();
        Username = username;
        PasswordHash = CryptographyService.HassPassword(password);
        Email = email;   
    }
}
