using TechSyence.Application.Extensions;
using TechSyence.Communication;
using TechSyence.Domain.Enums;
using TechSyence.Domain.Repositories;
using TechSyence.Domain.Repositories.User;
using TechSyence.Domain.Security.Cryptography;
using TechSyence.Domain.Security.Token;
using TechSyence.Domain.Services.LoggedUser;
using TechSyence.Exceptions;
using TechSyence.Exceptions.ExceptionsBase;
using Entity = TechSyence.Domain.Entities;

namespace TechSyence.Application.UseCases.User.Register;

public class RegisterUserUseCase(
    ILoggedUser loggedUser,
    IUserReadOnlyRepository readOnlyRepository,
    IUserWriteOnlyRepository writeOnlyRepository,
    IPasswordEncripter encripter,
    IAccessTokenGenerator accessTokenGenerator,
    IUnitOfWork unitOfWork
    ) : IRegisterUserUseCase
{

    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
    private readonly IPasswordEncripter _encripter = encripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IUnitOfWork _unityOfWork = unitOfWork;

    public async Task<ResponseRegisteredUser> Execute(RequestRegisterUser request)
    {
        var loggedUser = await _loggedUser.User();

        bool canCreate = CanCreateUser(loggedUser, (UserRolesEnum)request.Role);
        if (!canCreate)
            throw new NoPermissionException();

        await Validate(request);

        Entity.User user = request.ToUser();
        user.UpdatedOn = DateTime.UtcNow;
        user.LastLogin = DateTime.UtcNow;
        user.UserIndentifier = Guid.NewGuid();
        user.Password = _encripter.Encript(request.Password);
        user.CompanyId = loggedUser.CompanyId;

        await _writeOnlyRepository.RegisterUser(user);
        await _unityOfWork.Commit();

        return new ResponseRegisteredUser
        {
            FirstName = user.FirstName,
            Tokens = new ResponseToken
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIndentifier, user.Role, user.IsAdmin),
                RefreshToken = "Temporariamente desativado, para fins de desevolvimento"
            }
        };
    }

    private async Task Validate(RequestRegisterUser request)
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

    private static bool CanCreateUser(Entity.User loggedUser, UserRolesEnum userTargetRole)
    {
        if (loggedUser.IsAdmin || loggedUser.Role == UserRolesEnum.MANAGER)
            return true;

        return loggedUser.Role > userTargetRole &&
             userTargetRole != UserRolesEnum.CUSTOMER &&
             userTargetRole != UserRolesEnum.EMPLOYEE;
    }
}
