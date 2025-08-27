using TechSyence.Communication;

namespace TechSyence.Application.UseCases.Company.Register;
public interface IRegisterCompanyUseCase
{
    Task Execute(RequestRegisterCompany request);
}
