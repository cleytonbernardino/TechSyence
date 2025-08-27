using System.Security.Claims;
using TechSyence.Domain.Security.Token;
using TechSyence.Exceptions;
using TechSyence.Exceptions.ExceptionsBase;

namespace TechSyence.API.Token;

internal class HttpContextLoggedUser(
    IHttpContextAccessor httpContextAccessor
    ) : IAccessTokenClaims
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid GetUserIndentifier()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
            throw new TechSyenceException(ResourceMessagesException.USER_CONTEXT_NOT_FOUND);

        var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

        if (claim is null)
            throw new TechSyenceException(ResourceMessagesException.SID_CLAIM_NOT_FOUND);

        return Guid.Parse(claim);
    }
}
