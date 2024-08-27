namespace TMS.Security.Contracts.Requests;

/// <summary>
/// Модель запроса на вход.
/// </summary>
/// <param name="Username"> Логин. </param>
/// <param name="Password"> Пароль </param>
public record LoginRequest(string Username, string Password);