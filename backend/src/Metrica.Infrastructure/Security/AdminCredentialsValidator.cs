using System.Security.Cryptography;
using System.Text;
using Metrica.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Metrica.Infrastructure.Security;

public class AdminCredentialsValidator : IAdminCredentialsValidator
{
    private readonly AuthOptions _options;

    public AdminCredentialsValidator(IOptions<AuthOptions> options)
    {
        _options = options.Value;
    }

    public bool IsValid(string email, string password)
    {
        return FixedEquals(_options.AdminEmail, email.Trim())
            && FixedEquals(_options.AdminPassword, password);
    }

    private static bool FixedEquals(string expected, string actual)
    {
        var expectedBytes = Encoding.UTF8.GetBytes(expected);
        var actualBytes = Encoding.UTF8.GetBytes(actual);
        return CryptographicOperations.FixedTimeEquals(expectedBytes, actualBytes);
    }
}
