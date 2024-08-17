using TMS.Security.Core.Services;

namespace TMS.Security.Core;

public class User
{
    public Guid Id { get; private set; }

    public string Username { get; set; } = string.Empty;

    public byte[] PasswordHash { get; private set; }

    public byte[] PasswordSalt { get; private set; }

    public string Email { get; set; } = string.Empty;

    public User(string username, string password, string email)
    {
        Id = Guid.NewGuid();
        Username = username;
        SetPassword(password);
        Email = email;
    }

    public void SetPassword(string password)
    {
        (PasswordHash, PasswordSalt) = CryptographyService.CreatePasswordHash(password);
    }
}
