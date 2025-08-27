using TechSyence.Application.Extensions;
using TechSyence.Application.Services.Encoder;
using TechSyence.Communication;
using TechSyence.Domain.Repositories.Company;

namespace TechSyence.Application.UseCases.Company.List;

public class ListCompaniesUseCase(
    ICompanyReadOnlyRepository repository,
    IIdEncoder idEncoder
    ) : IListCompaniesUseCase
{
    private readonly ICompanyReadOnlyRepository _repository = repository;
    private readonly IIdEncoder _idEncoder = idEncoder;

    public ResponseShortCompanies Execute()
    {
        var companies = _repository.ListCompanies();

        var responseShortCompanies = new ResponseShortCompanies();

        foreach (var company in companies )
        {
            var shortCompany = company.ToResponse();
            shortCompany.Id = _idEncoder.Encode(company.Id);

            responseShortCompanies.Companies.Add(shortCompany);
        }

        return responseShortCompanies;
    }
}
