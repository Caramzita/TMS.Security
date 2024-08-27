namespace TMS.Security.Contracts.Requests;

/// <summary>
/// Модель запроса на смену пароля.
/// </summary>
/// <param name="Login"> Логин. </param>
/// <param name="Password"> Пароль </param>
/// <param name="NewPassword"> Новый пароль. </param>
public record ChangePasswordRequest(string Username, string Password, string NewPassword);
