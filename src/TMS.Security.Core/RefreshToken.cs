using System.Security.Cryptography;

namespace TMS.Security.Core;

/// <summary>
/// Представляет токен обновления, используемый для получения нового токена доступа.
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// Уникальный идентификатор токена обновления.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Сгенерированный токен обновления.
    /// </summary>
    public string Token { get; }

    /// <summary>
    /// Дата и время истечения срока действия токена.
    /// </summary>
    public DateTime Expires { get; }

    /// <summary>
    /// Определяет, был ли токен использован.
    /// </summary>
    public bool IsUsed {  get; private set; }

    /// <summary>
    /// Идентификатор пользователя, которому принадлежит токен обновления.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RefreshToken"/> 
    /// с указанной датой истечения срока действия и идентификатором пользователя.
    /// </summary>
    /// <param name="expires"> Дата и время истечения срока действия токена. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    public RefreshToken(DateTime expires, Guid userId)
    {
        Id = Guid.NewGuid();
        Token = GenerateToken();
        Expires = expires;
        UserId = userId;
    }

    /// <summary>
    /// Конструктор для инициализации модели уже существующего токена обновления.
    /// </summary>
    /// <param name="expires"> Дата и время истечения срока действия токена. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="isUsed"> Значение, указывающее, был ли токен использован. </param>
    public RefreshToken(Guid id, string token, DateTime expires, Guid userId, bool isUsed) 
        : this(expires, userId)
    {
        Id = id;
        Token = token;
        IsUsed = isUsed;
    }

    /// <summary>
    /// Генерирует случайный токен обновления.
    /// </summary>
    /// <returns> Сгенерированный токен в виде строки. </returns>
    private static string GenerateToken()
    {
        byte[] tokenData = new byte[32];
        RandomNumberGenerator.Fill(tokenData);
        return Convert.ToBase64String(tokenData);
    }

    /// <summary>
    /// Проверяет, действителен ли токен.
    /// </summary>
    /// <returns> Значение <see langword="true"/>, если токен действителен; 
    /// в противном случае <see langword="false"/>. </returns>
    public bool IsValid()
    {
        return !IsUsed && Expires > DateTime.UtcNow;
    }

    /// <summary>
    /// Деактивирует токен, помечая его как использованный.
    /// </summary>
    public void Deactivate()
    {
        IsUsed = true;
    }
}