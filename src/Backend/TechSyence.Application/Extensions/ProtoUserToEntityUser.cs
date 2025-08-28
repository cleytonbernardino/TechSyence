using TechSyence.Communication;
using TechSyence.Domain.Entities;

namespace TechSyence.Application.Extensions;

public static class ProtoUserToEntityUser
{
    public static ResponseShortUser ToShortResponse(this ShortUser user)
    {
        return new ResponseShortUser
        {
            FirstName = user.FirstName,
            LastName = user.LastName ?? "",
            LastLogin = user.LastLogin.ToString() ?? "",
            Role = user.Role.ToString(),
        };
    }
}
