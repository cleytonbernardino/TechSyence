using Shouldly;
using TechSyence.Infrastructure.Security.Cryptography;

namespace Validators.Test.Cryptography;

public class EncripterTest
{
    [Fact]
    public void Success()
    {
        var encripter = new Sha512Encripter("TestingValue");

        string password = "TestPassword";

        var encriptedPassword = encripter.Encript(password);

        encriptedPassword.ShouldNotBe(password);
    }
}
