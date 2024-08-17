namespace TMS.Security.Contracts.Requests;

public record ChangePasswordRequest(string Login, string Password, string NewPassword);
