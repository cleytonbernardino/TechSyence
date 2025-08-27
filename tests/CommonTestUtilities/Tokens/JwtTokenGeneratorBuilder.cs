using TechSyence.Domain.Security.Token;
using TechSyence.Infrastructure.Security.Token.Access.Generate;

namespace CommonTestUtilities.Tokens;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: "05F37Y6zj7HO57mEFXIalP1nj0EHiLs4", issuer: @"https://localhost:7055/");
}
