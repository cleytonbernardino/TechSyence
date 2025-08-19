using TechSyence.Communication;

namespace TechSyence.Application.UseCases.Login;

public interface IDoLoginUseCase
{
    Task<ResponseResgisteredUserJson> Execute(RequestLoginJson request);
}
