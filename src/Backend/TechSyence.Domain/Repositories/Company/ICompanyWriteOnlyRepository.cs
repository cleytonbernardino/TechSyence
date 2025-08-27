using Entity = TechSyence.Domain.Entities;

namespace TechSyence.Domain.Repositories.Company;

public interface ICompanyWriteOnlyRepository
{
    Task RegisterCompany(Entity.Company company);
}
