namespace TechSyence.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    Task<Entities.User?> GetById(long id, long comapanyId);
    void UpdateUser(Entities.User user);
}
