using System.Security.Cryptography;

namespace TMS.Security.Core;

public class RefreshToken
{
    public Guid Id { get; }

    public string Token { get; }

    public DateTime Expires { get; }

    public bool IsUsed {  get; private set; }

    public Guid UserId { get; }

    public RefreshToken(DateTime expires, Guid userId)
    {
        Id = Guid.NewGuid();
        Token = GenerateToken();
        Expires = expires;
        UserId = userId;
    }

    public RefreshToken(DateTime expires, Guid userId, bool isUsed) 
        : this(expires, userId)
    {
        IsUsed = isUsed;
    }

    private string GenerateToken()
    {
        byte[] tokenData = new byte[32];
        RandomNumberGenerator.Fill(tokenData);
        return Convert.ToBase64String(tokenData);
    }

    public bool IsValid()
    {
        return !IsUsed && Expires > DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsUsed = true;
    }
}