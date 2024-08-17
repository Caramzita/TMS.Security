namespace TMS.Security.DataAccess.Dto;

public class RefreshTokenDto
{
    public Guid Id { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime Expires { get; set; }

    public bool IsUsed { get; set; }

    public Guid UserId { get; set; }
}
