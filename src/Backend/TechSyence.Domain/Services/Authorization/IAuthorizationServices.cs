using TechSyence.Domain.Entities;

namespace TechSyence.Domain.Services.Authorization;

public interface IAuthorizationServices
{
    bool CanCreateUser(User user);
    bool CanReadUsers(User user);
    bool CanUpdateUser(User user);
    bool CanUpdateUserRole(User user);
    bool CanDeleteUser(User user);
}
