namespace TMS.Security.Contracts;

public sealed record Tokens(string AccessToken, string RefreshToken, DateTimeOffset RefreshTokenExpiration);
