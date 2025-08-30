using TechSyence.Communication;

namespace TechSyence.Application.UseCases.User.Delete;

public interface IDeleteUserUseCase
{
    Task Execute(long userToDeleteId);
}
