using TechSyence.Communication;

namespace TechSyence.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUser> Execute(RequestRegisterUser request);
}
