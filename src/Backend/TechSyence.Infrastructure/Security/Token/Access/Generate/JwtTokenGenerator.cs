using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TechSyence.Domain.Enums;
using TechSyence.Domain.Security.Token;

namespace TechSyence.Infrastructure.Security.Token.Access.Generate;

public class JwtTokenGenerator(
    uint expirationTimeMinutes,
    string signingKey,
    string issuer
    ) : JwtTokenHandle, IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes = expirationTimeMinutes;
    private readonly string _signingKey = signingKey;
    private readonly string _issuer = issuer;

    public string Generate(Guid userIndentifier, UserRolesEnum role, bool isAdmin = false)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Sid, userIndentifier.ToString()),
            new Claim(ClaimTypes.Role, (isAdmin ?  "ADMIN" : role.ToString()))
        };

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _issuer
        };

        JwtSecurityTokenHandler tokenHandle = new();
        SecurityToken securityToken = tokenHandle.CreateToken(tokenDescriptor);

        return tokenHandle.WriteToken(securityToken);
    }
}
