using TechSyence.Domain.Entities;

namespace TechSyence.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    Task<User> User();
}
