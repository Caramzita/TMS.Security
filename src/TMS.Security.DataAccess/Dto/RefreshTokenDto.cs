namespace TMS.Security.DataAccess.Dto;

/// <summary>
/// Представляет DTO для токена обновления.
/// </summary>
public class RefreshTokenDto
{
    /// <summary>
    /// Уникальный идентификатор токена обновления.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Значение токена обновления.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Дата и время истечения срока действия токена.
    /// </summary>
    public DateTime Expires { get; set; }

    /// <summary>
    /// Указывает, был ли токен использован.
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// Идентификатор пользователя, к которому относится токен обновления.
    /// </summary>
    public Guid UserId { get; set; }
}