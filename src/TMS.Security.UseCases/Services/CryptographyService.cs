using System.Security.Cryptography;
using System.Text;

namespace TMS.Security.UseCases.Services;

public static class CryptographyService
{
    public static string HassPassword(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var passwordData = SHA256.HashData(passwordBytes);

        return BitConverter.ToString(passwordData).Replace("-", string.Empty);
    }
}
