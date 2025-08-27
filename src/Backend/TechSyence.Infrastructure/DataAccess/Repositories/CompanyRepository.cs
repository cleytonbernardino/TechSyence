using Microsoft.EntityFrameworkCore;
using TechSyence.Domain.Entities;
using TechSyence.Domain.Repositories.Company;

namespace TechSyence.Infrastructure.DataAccess.Repositories;

public class CompanyRepository(
    TechSyenceDbContext dbContext
    ) : ICompanyWriteOnlyRepository, ICompanyReadOnlyRepository
{
    private readonly TechSyenceDbContext _dbContext = dbContext;

    public IList<ShortCompany> ListCompanies()
    {
        return _dbContext
               .Companies
               .AsNoTracking()
               .Select(field => new ShortCompany
               {
                   Id = field.Id,
                   DoingBusinessAs = field.LegalName,
                   SubscriptionStatus = field.SubscriptionStatus,
               }).ToList();
    }

    public async Task RegisterCompany(Company company)
    {
        await _dbContext
                .Companies
                .AddAsync(company);
    }
}
