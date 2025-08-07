namespace TechSyence.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}
