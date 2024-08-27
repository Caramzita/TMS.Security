namespace TMS.Security.Contracts.Requests;

/// <summary>
/// Модель запроса для регистрации.
/// </summary>
/// <param name="Username"> Имя пользователя. </param>
/// <param name="Password"> Пароль. </param>
/// <param name="Email"> Почта. </param>
public record RegisterRequest(string Username, string Password, string Email);
