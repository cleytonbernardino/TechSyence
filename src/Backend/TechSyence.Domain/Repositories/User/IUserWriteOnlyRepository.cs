namespace TechSyence.Domain.Repositories.User;

public interface IUserWriteOnlyRepository
{
    Task RegisterUser(Domain.Entities.User user);
}
