using Konscious.Security.Cryptography;
using TechSyence.Domain.Security.Cryptography;
using TechSyence.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build() => new Argon2Encripter();
}
