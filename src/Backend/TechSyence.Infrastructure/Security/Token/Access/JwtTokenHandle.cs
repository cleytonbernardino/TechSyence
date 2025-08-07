using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TechSyence.Infrastructure.Security.Token.Access;

internal abstract class JwtTokenHandle
{
    protected static SymmetricSecurityKey SecurityKey(string signingKey)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(signingKey);
        return new SymmetricSecurityKey(bytes);
    }
}
