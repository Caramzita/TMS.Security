namespace TMS.Security.Contracts.Requests;

/// <summary>
/// Модель запроса для обновления токена.
/// </summary>
/// <param name="RefreshToken"> Токен обновления. </param>
public record RefreshTokenRequest(string RefreshToken);
