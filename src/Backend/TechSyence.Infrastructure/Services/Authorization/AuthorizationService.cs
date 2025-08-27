using TechSyence.Domain.Entities;
using TechSyence.Domain.Enums;
using TechSyence.Domain.Services.Authorization;

namespace TechSyence.Infrastructure.Services.Authorization;

public class AuthorizationService : IAuthorizationServices
{
    private static bool HasSpecialPermission(User user)
    {
        return user.IsAdmin ||
            user.Role == UserRolesEnum.MANAGER ||
            user.Role == UserRolesEnum.SUB_MANAGER ||
            user.Role == UserRolesEnum.RH;
    }

    public bool CanCreateUser(User user)
    {
        if ( HasSpecialPermission(user))
            return true;

        return false;
    }

    public bool CanDeleteUser(User user)
    {
        if(HasSpecialPermission(user))
            return true;

        return false;
    }

    public bool CanReadUsers(User user) => HasSpecialPermission(user);

    public bool CanUpdateUser(User user) => HasSpecialPermission(user);

    public bool CanUpdateUserRole(User user)
    {
        if (user.Role == UserRolesEnum.RH)
            return false;

        if (HasSpecialPermission(user))
            return true;

        return false;
    }
}
