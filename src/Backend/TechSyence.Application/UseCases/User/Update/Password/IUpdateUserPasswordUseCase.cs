using TechSyence.Communication;

namespace TechSyence.Application.UseCases.User.Update.Password;

public interface IUpdateUserPasswordUseCase
{
    Task Execute(RequestUpdateUserPassword request);
}
