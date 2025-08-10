using System.Security.Cryptography;
using System.Text;
using TechSyence.Domain.Security.Cryptography;

namespace TechSyence.Infrastructure.Security.Cryptography;

public class Sha512Encripter(
    string salt
    ) : IPasswordEncripter
{
    private readonly string _salt = salt;

    public string Encript(string password)
    {
        string newPassword = Encoding.UTF8 + _salt;

        byte[] bytes = Encoding.UTF8.GetBytes(newPassword);
        byte[] hashBytes = SHA512.HashData(bytes);

        return StringBytes(hashBytes);
    }

    private static string StringBytes(byte[] bytes)
    {
        StringBuilder sb = new();
        foreach (byte b in bytes)
        {
            string hex = b.ToString("x2");
            sb.Append(hex);
        }
        return sb.ToString();
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var encriptedPassword = Encript(password);
        return encriptedPassword == hashedPassword;
    }
}
