using System.Security.Cryptography;
using System.Text;

namespace TMS.Security.Core.Services;

public static class CryptographyService
{
    public static (byte[] PasswordHash, byte[] PasswordSalt) CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        var passwordSalt = hmac.Key;
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password + Convert.ToBase64String(passwordSalt)));
        return (passwordHash, passwordSalt);
    }

    public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password + Convert.ToBase64String(storedSalt)));
        return computedHash.SequenceEqual(storedHash);
    }
}
