namespace Metrica.Application.Handlers.Commands.Auth;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;

    public int ExpiresIn { get; set; }

    public AuthenticatedUser User { get; set; } = new();
}

public class AuthenticatedUser
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
