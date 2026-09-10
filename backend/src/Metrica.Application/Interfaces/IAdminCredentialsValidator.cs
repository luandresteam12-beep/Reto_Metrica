namespace Metrica.Application.Interfaces;

public interface IAdminCredentialsValidator
{
    bool IsValid(string email, string password);
}
