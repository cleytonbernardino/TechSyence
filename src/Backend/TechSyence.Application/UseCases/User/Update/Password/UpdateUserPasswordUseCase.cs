using TechSyence.Communication;
using TechSyence.Domain.Repositories;
using TechSyence.Domain.Repositories.User;
using TechSyence.Domain.Security.Cryptography;
using TechSyence.Domain.Services.LoggedUser;
using TechSyence.Exceptions;
using TechSyence.Exceptions.ExceptionsBase;

namespace TechSyence.Application.UseCases.User.Update.Password;

public class UpdateUserPasswordUseCase(
    ILoggedUser loggedUser,
    IUserUpdateOnlyRepository repository,
    IPasswordEncripter passwordEncryption,
    IUnitOfWork unitOfWork
    ) : IUpdateUserPasswordUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserUpdateOnlyRepository _repository = repository;
    private readonly IPasswordEncripter _passwordEncryption = passwordEncryption;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task Execute(RequestUpdateUserPassword request)
    {
        var loggedUser = await _loggedUser.User();
        Validate(request);

        var user = await _repository.GetUserByEmailAndPassword(loggedUser.Email, request.OldPassword);
        if (user is null)
            throw new NoPermissionException(ResourceMessagesException.CURRENT_PASSWORD_INCORRECT);

        string encryptedPassword = _passwordEncryption.Encript(request.NewPassword);
        user.Password = encryptedPassword;
        user.UpdatedOn = DateTime.UtcNow;
        
        _repository.UpdateUser(user);
        await _unitOfWork.Commit();
    }
    
    private static void Validate(RequestUpdateUserPassword request)
    {
        var validator = new UpdateUserPasswordValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
            throw new ErrorOnValidationException(
                validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
    }
}
