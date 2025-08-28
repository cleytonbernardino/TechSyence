using TechSyence.Domain.Entities;

namespace TechSyence.Domain.Repositories.Company;

public interface ICompanyReadOnlyRepository
{
    IList<ShortCompany> ListCompanies();
    IList<ShortUser> ListUsers(long companyId);
}
