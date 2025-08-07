using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TechSyence.Domain.Security.Token;

namespace TechSyence.Infrastructure.Security.Token.Access.Validator;

internal class JwtTokenValidator(
    string signingKey
    ) : JwtTokenHandle, IAccessTokenValidator
{
    private readonly string _signingKey = signingKey;

    public Guid ValidateAndGetUserIdentifier(string token)
    {
        TokenValidationParameters validationParameter = new()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = SecurityKey(_signingKey),
            ClockSkew = new TimeSpan(0)
        };

        JwtSecurityTokenHandler tokenHandle = new();

        ClaimsPrincipal claimsPrincipal = tokenHandle.ValidateToken(token, validationParameter, out _);

        string userIndentifier = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        return Guid.Parse(userIndentifier);
    }
}
