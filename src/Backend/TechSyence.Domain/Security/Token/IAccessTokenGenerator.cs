using TechSyence.Domain.Enums;

namespace TechSyence.Domain.Security.Token;

public interface IAccessTokenGenerator
{
    string Generate(Guid userIndentifier, UserRolesEnum role, bool isAdmin = false);
}
