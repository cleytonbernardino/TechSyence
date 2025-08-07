using TechSyence.Communiction.Requests;
using TechSyence.Communiction.Responses;

namespace TechSyence.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseResgisteredUserJson> Execute(RequestRegisterUserJson request);
}
