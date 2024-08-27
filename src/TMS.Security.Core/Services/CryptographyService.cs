using System.Security.Cryptography;
using System.Text;

namespace TMS.Security.Core.Services;

/// <summary>
/// Предоставляет методы для создания и проверки хэшей паролей.
/// </summary>
public static class CryptographyService
{
    /// <summary>
    /// Создает хэш пароля и соль для его защиты.
    /// </summary>
    /// <param name="password"> Пароль, который нужно захешировать. </param>
    /// <returns> Кортеж, содержащий хэш пароля и соль. </returns>
    public static (byte[] PasswordHash, byte[] PasswordSalt) CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        var passwordSalt = hmac.Key;
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password + Convert.ToBase64String(passwordSalt)));
        return (passwordHash, passwordSalt);
    }

    /// <summary>
    /// Проверяет правильность введенного пароля, сравнивая его с сохраненным хэшем и солью.
    /// </summary>
    /// <param name="password"> Пароль, который нужно проверить. </param>
    /// <param name="storedHash"> Сохраненный хэш пароля для сравнения. </param>
    /// <param name="storedSalt"> Сохраненная соль пароля для сравнения. </param>
    /// <returns> Значение <see langword="true"/>, если пароль корректный; в противном случае <see langword="false"/>. </returns>
    public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password + Convert.ToBase64String(storedSalt)));
        return computedHash.SequenceEqual(storedHash);
    }
}
