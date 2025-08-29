using TechSyence.Communication;

namespace TechSyence.Application.UseCases.Login;

public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUser> Execute(RequestLogin request);
}
