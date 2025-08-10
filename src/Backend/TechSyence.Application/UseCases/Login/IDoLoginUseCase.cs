using TechSyence.Communiction.Requests;
using TechSyence.Communiction.Responses;

namespace TechSyence.Application.UseCases.Login;

public interface IDoLoginUseCase
{
    Task<ResponseResgisteredUserJson> Execute(RequestLoginJson request);
}
