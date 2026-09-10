namespace Metrica.Application.Interfaces;

public class AccessToken
{
    public string Token { get; set; } = string.Empty;

    public int ExpiresIn { get; set; }
}

public interface ITokenService
{
    AccessToken Create(int userId, string email, string role);
}
