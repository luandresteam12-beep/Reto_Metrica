using MediatR;
using Metrica.Application.Common.Exceptions;
using Metrica.Application.Interfaces;

namespace Metrica.Application.Handlers.Commands.Auth;

public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly IAdminCredentialsValidator _credentialsValidator;
    private readonly ITokenService _tokenService;

    public LoginHandler(IAdminCredentialsValidator credentialsValidator,ITokenService tokenService)
    {
        _credentialsValidator = credentialsValidator;
        _tokenService = tokenService;
    }

    public Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        if (!_credentialsValidator.IsValid(request.Email, request.Password))
        {
            throw new InvalidCredentialsException();
        }

        var accessToken = _tokenService.Create(1, request.Email.Trim().ToLowerInvariant(), "Admin");
        return Task.FromResult(new LoginResponse
        {
            Token = accessToken.Token,
            ExpiresIn = accessToken.ExpiresIn,
            User = new AuthenticatedUser
            {
                Id = 1,
                Email = request.Email.Trim().ToLowerInvariant(),
                Role = "Admin"
            }
        });
    }
}
