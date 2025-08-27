using Entity = TechSyence.Domain.Entities;
namespace TechSyence.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<bool> ExistActiveUserWithId(long id);
    Task<Entity.User?> GetUserByEmailAndPassword(string email, string password);
    Task<bool> ExistActiveUserWithIndentifier(Guid userIndentifier);
}
