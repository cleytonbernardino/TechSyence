using Moq;
using TechSyence.Domain.Entities;
using TechSyence.Domain.Repositories.Company;

namespace CommonTestUtilities.Repositories;

public class CompanyWriteOnlyRepositoryBuilder
{
    public static ICompanyWriteOnlyRepository Build()
    {
        Mock<ICompanyWriteOnlyRepository> mock = new();
        return mock.Object;
    }
}
