using TechSyence.Application.Extensions;
using TechSyence.Communiction.Requests;
using TechSyence.Communiction.Responses;
using TechSyence.Domain.Repositories;
using TechSyence.Domain.Repositories.User;
using TechSyence.Domain.Security.Cryptography;
using TechSyence.Domain.Security.Token;
using TechSyence.Exceptions;
using TechSyence.Exceptions.ExceptionsBase;
using Entity = TechSyence.Domain.Entities;

namespace TechSyence.Application.UseCases.User.Register;

public class RegisterUserUseCase(
    IUserReadOnlyRepository readOnlyRepository,
    IUserWriteOnlyRepository writeOnlyRepository,
    IPasswordEncripter encripter,
    IAccessTokenGenerator accessTokenGenerator,
    IUnitOfWork unitOfWork
    ) : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
    private readonly IPasswordEncripter _encripter = encripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IUnitOfWork _unityOfWork = unitOfWork;

    public async Task<ResponseResgisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        Entity.User user = request.ToUser();
        user.UpdatedOn = DateTime.UtcNow;
        user.UserIndentifier = Guid.NewGuid();
        user.Password = _encripter.Encript(request.Password);

        await _writeOnlyRepository.RegisterUser(user);
        await _unityOfWork.Commit();

        return new ResponseResgisteredUserJson
        {
            FirstName = user.FirstName,
            Tokens = new ResponseTokenJson
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIndentifier)
            }
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        RegisterUserValidator validator = new();
        var result = validator.Validate(request);

        bool emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("", ResourceMessagesException.EMAIL_IN_USE));

        if ( !result.IsValid )
            throw new ErrorOnValidationException(
                result.Errors.Select(err => err.ErrorMessage).ToList()
            );
    }
}
