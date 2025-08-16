using TechSyence.Communication.Requests;
using TechSyence.Communication.Responses;

namespace TechSyence.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseResgisteredUserJson> Execute(RequestRegisterUserJson request);
}
