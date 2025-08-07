namespace TechSyence.Domain.Security.Cryptography;

public interface IPasswordEncripter
{
    string Encript(string password);
}
