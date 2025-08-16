using TechSyence.Communication.Requests;
using TechSyence.Communication.Responses;

namespace TechSyence.Application.UseCases.Login;

public interface IDoLoginUseCase
{
    Task<ResponseResgisteredUserJson> Execute(RequestLoginJson request);
}
