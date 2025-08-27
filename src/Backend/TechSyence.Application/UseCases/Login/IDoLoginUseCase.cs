using TechSyence.Communication;

namespace TechSyence.Application.UseCases.Login;

public interface IDoLoginUseCase
{
    Task<ResponseResgisteredUser> Execute(RequestLogin request);
}
