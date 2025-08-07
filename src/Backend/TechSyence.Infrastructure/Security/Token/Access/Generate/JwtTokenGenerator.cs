using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TechSyence.Domain.Security.Token;
using TechSyence.Infrastructure.Security.Token.Access;

namespace TechSyence.Infrastructure.Security.Token.Access.Generate;

internal class JwtTokenGenerator(
    uint expirationTimeMinutes,
    string signingKey
    ) : JwtTokenHandle, IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes = expirationTimeMinutes;
    private readonly string _signingKey = signingKey;

    public string Generate(Guid userIndentifier)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Sid, userIndentifier.ToString())
        };

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature)
        };

        JwtSecurityTokenHandler tokenHandle = new();
        SecurityToken securityToken = tokenHandle.CreateToken(tokenDescriptor);

        return tokenHandle.WriteToken(securityToken);
    }
}
