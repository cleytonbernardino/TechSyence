using Shouldly;
using TechSyence.Infrastructure.Security.Cryptography;

namespace Validators.Test.Cryptography;

public class EncripterTest
{
    [Fact]
    public void Success()
    {
        var encripter = new Argon2Encripter();

        string password = "TestPassword";

        var encriptedPassword = encripter.Encript(password);

        encriptedPassword.ShouldNotBe(password);
    }

    [Fact]
    public void Success_Verify_Password()
    {
        var encripter = new Argon2Encripter();

        string password = "TestPassword";

        var encriptedPassword = encripter.Encript(password);

        encripter.VerifyPassword(password, encriptedPassword).ShouldBeTrue();
    }

    [Fact]
    public void Error_Incorrect_Password()
    {
        var encripter = new Argon2Encripter();

        string password = "TestPassword";
        string incorretPassword = "TestPassword123";

        var encriptedPassword = encripter.Encript(password);

        encripter.VerifyPassword(incorretPassword, encriptedPassword).ShouldBeFalse();
    }
}
