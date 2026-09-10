using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Metrica.Application.Handlers.Commands.Auth;

public class LoginRequest : IRequest<LoginResponse>
{
    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(128, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}
