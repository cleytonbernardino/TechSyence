using TechSyence.Communication;

namespace TechSyence.Application.UseCases.Company.ListUsers;

public interface IListCompanyUsersUseCase
{
    Task<ResponseListCompanyUser> Execute();
}
