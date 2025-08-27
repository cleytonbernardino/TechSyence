using TechSyence.Application.Extensions;
using TechSyence.Application.Services.Encoder;
using TechSyence.Communication;
using TechSyence.Domain.Repositories;
using TechSyence.Domain.Repositories.Company;
using TechSyence.Domain.Services.LoggedUser;
using TechSyence.Exceptions.ExceptionsBase;

namespace TechSyence.Application.UseCases.Company.Register;

public class RegisterCompanyUseCase(
    IIdEncoder idEncoder,
    ILoggedUser loggedUser,
    ICompanyWriteOnlyRepository repository,
    IUnitOfWork unitOfWork
    ) : IRegisterCompanyUseCase
{
    private readonly IIdEncoder _idEncoder = idEncoder;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ICompanyWriteOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(RequestRegisterCompany request)
    {
        var loggedUser = await _loggedUser.User();

        if (loggedUser.IsAdmin == false)
            throw new NoPermission();

        await Validator(request);

        var company = request.ToCompany();
        
        company.UpdatedOn = DateTime.UtcNow;
        company.Active = true;
        if (!string.IsNullOrEmpty(request.SubscriptionPlan))
            company.SubscriptionPlan = (short)_idEncoder.Decode(request.SubscriptionPlan);

        await _repository.RegisterCompany(company);
        await _unitOfWork.Commit();
    }

    private async Task Validator(RequestRegisterCompany request)
    {
        RegisterCompanyValidator validator = new();

        var result = await validator.ValidateAsync(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(err => err.ErrorMessage).ToArray());
    }
}
