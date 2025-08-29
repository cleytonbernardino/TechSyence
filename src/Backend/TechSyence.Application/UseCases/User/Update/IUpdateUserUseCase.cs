using TechSyence.Communication;

namespace TechSyence.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUser request);
}
