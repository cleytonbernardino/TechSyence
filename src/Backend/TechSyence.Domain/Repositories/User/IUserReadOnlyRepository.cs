using Entity = TechSyence.Domain.Entities;
namespace TechSyence.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<Entity.User?> GetUserByEmailAndPassword(string email, string password);
}
